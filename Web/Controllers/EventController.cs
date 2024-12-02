using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using Web.Attributes;
using Web.DataBaseContext;
using Web.DTOs;
using Web.Entities;

namespace Web.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController(VideoAnalisysDBContext dbContext) : ControllerBase
    {
        private readonly VideoAnalisysDBContext _dbContext = dbContext;

        [HttpGet("events")]
        public async Task<ActionResult<List<EventVM>>> GetEvents()
        {
            var Events = await _dbContext
                .Events
                .Include(x=>x.VideoFile)
                .AsNoTracking()
                .ToListAsync();

            List<EventVM> eventsVM = Events.Select(x => new EventVM().ConvertToEventVM(x)).ToList();

            return Ok(eventsVM);
        }

        [HttpPost("event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreatedEventVM eventVM)
        {
            if(eventVM.Name.Length > 127)
            {
                return BadRequest("Название мероприятия не может быть длинее 127 символов");
            }

            Event NewEvent = new()
            {
                Name = eventVM.Name,
                DateTime = eventVM.DateTime.ToUniversalTime(),
                Description = eventVM.Description
            };

            _dbContext.Events.Add(NewEvent);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("${eventID}")]
        public async Task<IActionResult> EditEvent(long eventID, [FromBody]CreatedEventVM editedEventVM)
        {
            if (editedEventVM.Name.Length > 127)
            {
                return BadRequest("Название мероприятия не может быть длинее 127 символов");
            }

            var editedEvent = await _dbContext
                .Events
                .Where(x => x.EventID == eventID)
                .FirstOrDefaultAsync();

            if (editedEvent == null)
            {
                return NotFound("Мероприятие не найдено");
            }

            editedEvent.Name = editedEventVM.Name;
            editedEvent.DateTime = editedEventVM.DateTime;
            editedEvent.Description = editedEventVM.Description;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("${eventID}")]
        public async Task<IActionResult> DeleteEvent(long eventID)
        {
            var deletedEvent = await _dbContext
                .Events
                .Where(x => x.EventID == eventID)
                .FirstOrDefaultAsync();

            if (deletedEvent == null)
            {
                return NotFound("Мероприятие не найдено");
            }

            _dbContext.Events.Remove(deletedEvent);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L; // 10GB
        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        [HttpPost("${eventID}/videoFile")]
        public async Task<IActionResult> AddVideoFileToEvent(long eventID, IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                return BadRequest("Файл отсутствует или пустой");
            }
            if (videoFile.ContentType != "video/mp4")
            {
                return BadRequest("Неверный формат файла. Допустимы только видеофайлы в формате MP4");
            }

            var existingEvent = await _dbContext
                .Events
                .Include(x=>x.VideoFile)
                .Where(x => x.EventID == eventID)
                .FirstOrDefaultAsync();

            if (existingEvent == null)
            {
                return NotFound("Мероприятие не найдено");
            }
            if (existingEvent.VideoFile != null)
            {
                return BadRequest("У мероприятия уже есть видеофайл");
            }

            string endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT")!;
            string accessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY")!;
            string secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY")!;
            string bucketName = Environment.GetEnvironmentVariable("MINIO_BUCKET")!;

            var minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();

            string filename = $"event{eventID}/{videoFile.FileName}";

            try
            {
                using var stream = videoFile.OpenReadStream();
                await minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filename)
                    .WithStreamData(stream)
                    .WithObjectSize(videoFile.Length)
                    .WithContentType(videoFile.ContentType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при загрузке файла в MinIO: {ex.Message}");
            }

            MinioFile video = new()
            {
                Name = videoFile.Name,
                Size = videoFile.Length,
                CreatedAt = DateTime.UtcNow,
                MimeType = videoFile.ContentType,
                Path = filename
            };

            existingEvent.VideoFile = video;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("${eventID}/expectedEmployees")]
        public async Task<IActionResult> AddEmployeesToEvent(long eventID, [FromBody] List<long> employees)
        {
            var existingEvent = await _dbContext
                .Events
                .Include(x => x.ExpectedEmployees)
                .Where(x => x.EventID == eventID)
                .FirstOrDefaultAsync();

            if (existingEvent == null)
            {
                return NotFound("Мероприятие не найдено");
            }

            if (employees.Count != 0)
            {
                var existingEmployees = await _dbContext
                    .Employees
                    .Where(x=> employees.Contains(x.EmployeeID))
                    .AsNoTracking()
                    .ToListAsync();

                if (existingEmployees.Count != employees.Count)
                {
                    return NotFound("Не все работники существуют");
                }

                existingEvent.ExpectedEmployees = existingEmployees;
            }
            else
            {
                existingEvent.ExpectedEmployees.Clear();
            }

            await _dbContext.SaveChangesAsync();

            return Ok();

        }
    }
}
