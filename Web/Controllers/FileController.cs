using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using System.Xml.Linq;
using Web.Attributes;
using Web.DataBaseContext;
using Web.DTOs;
using Web.Entities;
using Web.Service;

namespace Web.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController(VideoAnalisysDBContext dbContext) : ControllerBase
    {
        private readonly VideoAnalisysDBContext _dbContext = dbContext;

        [HttpGet("{fileID}")]
        public async Task<IActionResult> GetFile(long fileID)
        {
            var existingFile = await _dbContext
                .Files
                .Where(x => x.FileID == fileID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (existingFile == null)
            {
                return NotFound("Файл не существует");
            }

            string endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT")!;
            string accessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY")!;
            string secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY")!;
            string bucketName = Environment.GetEnvironmentVariable("MINIO_BUCKET")!;

            var minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();

            string filename = existingFile.Path;

            var res = new ReleaseableFileStreamModel();
            var getArgs = new GetObjectArgs()
                .WithObject(filename)
                .WithBucket(bucketName)
                .WithCallbackStream(res.SetStreamAsync);
            try
            {
                await res.HandleAsync(minioClient.GetObjectAsync(getArgs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении файла из MinIO: {ex.Message}");
            }

            return File(res.Stream, existingFile.MimeType, existingFile.Name);
        }
    }
}
