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
        public async Task<ActionResult<PaginatedVM<EventVM>>> GetEvents(int page = 1, int quantityPerPage = 9, string? search = null) //TODO: сделоть погинацио
        {
            if (page < 1)
            {
                page = 1;
            }

            var queryEvent = _dbContext.Events.AsQueryable();

            if (search != null)
            {
                queryEvent = queryEvent.Where(x => EF.Functions.ILike(x.Name, $"%{search}%"));
            }

            var eventsCount = await queryEvent.CountAsync();

            if ((page-1)*quantityPerPage >= eventsCount)
            {
                page = (int)Math.Ceiling((double)eventsCount/quantityPerPage);
            }

            var Events = await queryEvent
                .AsNoTracking()
                .Skip((page - 1) * quantityPerPage)
                .Take(quantityPerPage)
                .ToListAsync();

            List<EventVM> eventsVM = Events.Select(x => new EventVM().ConvertToEventVM(x)).ToList();

            foreach(var eventVM in eventsVM)
            {
                var presentEmployeesDB = await _dbContext
                    .EmployeeMarks
                    .Where(x => x.EventID == eventVM.EventID)
                    .GroupBy(x => x.EmployeeID)
                    .Select(x => x.First()).CountAsync();

                var unregisterPersonsDB = await _dbContext
                    .UnregisterPersonMarks
                    .Where(x => x.EventID == eventVM.EventID)
                    .GroupBy(x => x.UnregisterPersonID)
                    .Select(x => x.First()).CountAsync();

                eventVM.VisitorsCount = presentEmployeesDB + unregisterPersonsDB;
            }

            PaginatedVM<EventVM> paginatedEventsVM = new()
            {
                Count = eventsCount,
                Page = page,
                Nodes = eventsVM
            };

            return Ok(paginatedEventsVM);
        }

        [HttpGet("{eventID}")]
        public async Task<ActionResult<CurrentEventVM>> GetEvent(long eventID)
        {
            var existingEvent = await _dbContext
                .Events
                .Include(x=>x.VideoFile)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EventID == eventID);

            if (existingEvent == null)
            {
                return NotFound("Мероприятие не найдено");
            }

            CurrentEventVM eventVM = new CurrentEventVM().ConvertToCurrentEventVM(existingEvent);
            return eventVM;
        }

        [HttpPost("event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreatedEventVM eventVM)
        {
            if (eventVM.Name.Length > 127)
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
        public async Task<IActionResult> EditEvent(long eventID, [FromBody] CreatedEventVM editedEventVM)
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
                .Include(x => x.VideoFile)
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
                Name = videoFile.FileName,
                Size = videoFile.Length,
                CreatedAt = DateTime.UtcNow,
                MimeType = videoFile.ContentType,
                Path = filename
            };

            existingEvent.VideoFile = video;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{eventID}/videoFile")]
        public async Task<IActionResult> RemoveVideoFileFromEvent(long eventID)
        {
            var existingEvent = await _dbContext
                .Events
                .Include(x => x.VideoFile)
                .Where(x => x.EventID == eventID)
                .FirstOrDefaultAsync();

            if (existingEvent == null)
            {
                return NotFound("Мероприятия не существует");
            }

            if (existingEvent.VideoFile == null)
            {
                return NotFound("У мероприятия нет видеофайла");
            }

            string endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT")!;
            string accessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY")!;
            string secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY")!;
            string bucketName = Environment.GetEnvironmentVariable("MINIO_BUCKET")!;

            var minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();

            string filename = existingEvent.VideoFile.Path;//$"event{eventID}/{existingEvent.VideoFile.Name}";

            try
            {
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filename));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении файла из MinIO: {ex.Message}");
            }

            _dbContext.Files.Remove(existingEvent.VideoFile);
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
                    .Where(x => employees.Contains(x.EmployeeID))
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

        [HttpGet("{eventID}/visitingStatistics")]
        public async Task<ActionResult<PresentAndAbsentEmployees>> GetVisitingStatistics(long eventID)
        {
            var existingEvent = await _dbContext
                .Events
                .Include(x => x.ExpectedEmployees)
                .ThenInclude(x => x.Biometrics)
                .Include(x=>x.ExpectedEmployees)
                .ThenInclude(x=>x.Post)
                .ThenInclude(x=>x.Department)
                .AsNoTracking()
                .FirstAsync(x => x.EventID == eventID);

            if (existingEvent == null)
            {
                return NotFound("Мероприятие не найдено");
            }

            var presentEmployeesDB = await _dbContext
                .EmployeeMarks
                .Include(x => x.Employee)
                .ThenInclude(y => y.Biometrics)
                .Include(x=>x.Employee)
                .ThenInclude(x=>x.Post)
                .ThenInclude(x=>x.Department)
                .Where(x=>x.EventID == eventID)
                .GroupBy(x => x.EmployeeID)
                .Select(x => x.First())
                .ToListAsync();

            var presentEmployees = presentEmployeesDB.Select(x => x.Employee).ToList();

            List<Employee>? absentEmployees = [];
            if (presentEmployees.Count != 0)
            {
                foreach(var emp in existingEvent.ExpectedEmployees)
                {
                    if (!presentEmployees.Select(x=>x.EmployeeID).Contains(emp.EmployeeID))
                    {
                        absentEmployees.Add(emp);
                    }
                }
            }
            else
            {
                absentEmployees = existingEvent?.ExpectedEmployees.ToList();
            }



            PresentAndAbsentEmployees statistics = new()
            {
                PresentEmployees = presentEmployees.Select(x => new EmployeeVM()
                {
                    EmployeeID = x.EmployeeID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Patronymic = x.Patronymic,
                    Post = x.Post.Name,
                    Department = x.Post.Department.Name,
                    Phone = x.Phone,
                    AvatarID = x.Biometrics.Count == 0? null : x.Biometrics.Select(x => x.FileID)?.First(),
                }).ToList(),

                AbsentEmployees = absentEmployees?.Select(x => new EmployeeVM()
                {
                    EmployeeID = x.EmployeeID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Patronymic = x.Patronymic,
                    Post = x.Post.Name,
                    Department = x.Post.Department.Name,
                    Phone = x.Phone,
                    AvatarID = x.Biometrics.Count == 0 ? null : x.Biometrics.Select(x => x.FileID)?.First(),
                }).ToList(),
            };

            return Ok(statistics);
        }
    }
}
