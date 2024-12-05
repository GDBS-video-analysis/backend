using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Args;
using Minio;
using Web.Attributes;
using Web.DataBaseContext;
using Web.DTOs;
using Web.Entities;

namespace Web.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController(VideoAnalisysDBContext dbContext) : ControllerBase
    {
        private readonly VideoAnalisysDBContext _dbContext = dbContext;

        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreatedEmployeeVM createdEmployee)
        {
            if (createdEmployee.FirstName.Length > 127)
            {
                return BadRequest("Имя не может содержать больше 127 символов");
            }
            if (createdEmployee.LastName.Length > 127)
            {
                return BadRequest("Фамилия не может содержать больше 127 символов");
            }
            if (createdEmployee.Patronymic?.Length > 127)
            {
                return BadRequest("Отчество не может содержать больше 127 символов");
            }
            if (createdEmployee.Phone.Length > 15)
            {
                return BadRequest("Номер телефона не может содержать больше 15 символов");
            }

            var existingPost = await _dbContext.Posts.Where(x => x.PostID == createdEmployee.PostID).FirstOrDefaultAsync();

            if (existingPost == null)
            {
                return NotFound("Должность не найдена");
            }

            Employee newEmployee = new()
            {
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                Phone = createdEmployee.Phone,
                Patronymic = createdEmployee.Patronymic,
                PostID = createdEmployee.PostID,
                IsDeleted = false
            };

            _dbContext.Employees.Add(newEmployee);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        private const long MaxFileSize = 15L * 1024L * 1024L; // 15MB
        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        [HttpPost("${employeeID}/biometry")]
        public async Task<IActionResult> AddVideoFileToEvent(long employeeID, IFormFile biometry)
        {
            if (biometry == null || biometry.Length == 0)
            {
                return BadRequest("Файл отсутствует или пустой");
            }

            string[] JIuCT3anpeLLLeHku = ["image/png", "image/jpeg"];

            if (!JIuCT3anpeLLLeHku.Contains(biometry.ContentType))
            {
                return BadRequest("Неверный формат файла. Допустимы только видеофайлы в формате png/jpg");
            }

            var existingEmployee = await _dbContext
                .Employees
                .Include(x => x.Biometrics)
                .Where(x => x.EmployeeID == employeeID && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (existingEmployee == null)
            {
                return NotFound("Сотрудник не найден");
            }

            string endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT")!;
            string accessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY")!;
            string secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY")!;
            string bucketName = Environment.GetEnvironmentVariable("MINIO_BUCKET")!;

            var minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();

            string filename = $"employee{employeeID}/{Guid.NewGuid()}-{biometry.FileName}";

            try
            {
                using var stream = biometry.OpenReadStream();
                await minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filename)
                    .WithStreamData(stream)
                    .WithObjectSize(biometry.Length)
                    .WithContentType(biometry.ContentType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при загрузке файла в MinIO: {ex.Message}");
            }

            MinioFile file = new()
            {
                Name = biometry.Name,
                Size = biometry.Length,
                CreatedAt = DateTime.UtcNow,
                MimeType = biometry.ContentType,
                Path = filename
            };

            existingEmployee.Biometrics.Add(file);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{employeeID}/biometry")]
        public async Task<IActionResult> RemoveVideoFileFromEvent(long employeeID, long biometryID)
        {
            var existingEmployee = await _dbContext
                .Employees
                .Include(x => x.Biometrics)
                .Where(x => x.EmployeeID == employeeID && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (existingEmployee == null)
            {
                return NotFound("Сотрудник не найден");
            }

            var existingFile = existingEmployee.Biometrics.FirstOrDefault(x => x.FileID == biometryID);

            if (existingFile == null)
            {
                return NotFound("Файл не найден");
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

            _dbContext.Files.Remove(existingFile);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{employeeID}/forEdit")]
        public async Task<ActionResult<EmployeeForEditVM>> GetEmployeeForEdit(long employeeID)
        {
            var existingEmployee = await _dbContext
                .Employees
                .Include(x => x.Biometrics)
                .Include(x => x.Post)
                .ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.EmployeeID == employeeID && x.IsDeleted == false);

            if (existingEmployee == null)
            {
                return NotFound("Сотрудник не найден");
            }

            EmployeeForEditVM employeeVM = new()
            {
                EmployeeID = existingEmployee.EmployeeID,
                FirstName = existingEmployee.FirstName,
                LastName = existingEmployee.LastName,
                Patronymic = existingEmployee.Patronymic,
                Phone = existingEmployee.Phone,
                PostID = existingEmployee.Post.PostID,
                DepartmentID = existingEmployee.Post.Department.DepartmentID,
                Biometrics = existingEmployee.Biometrics.Select(x => x.FileID).ToList()
            };

            return Ok(employeeVM);
        }

        [HttpGet("{employeeID}")]
        public async Task<ActionResult<EmployeeVM>> GetEmployee(long employeeID)
        {
            var existingEmployee = await _dbContext
                .Employees
                .Include(x => x.Post)
                .Include(x => x.Biometrics)
                .FirstOrDefaultAsync(x => x.EmployeeID == employeeID && x.IsDeleted == false);

            if (existingEmployee == null)
            {
                return NotFound("Сотрудник не найден");
            }

            long? biometryID = existingEmployee.Biometrics.FirstOrDefault()?.FileID;

            EmployeeVM employeeVM = new()
            {
                EmployeeID = existingEmployee.EmployeeID,
                FirstName = existingEmployee.FirstName,
                LastName = existingEmployee.LastName,
                Patronymic = existingEmployee.Patronymic,
                Post = existingEmployee.Post.Name,
                Phone = existingEmployee.Phone,
                Avatar = biometryID
            };

            return Ok(employeeVM);
        }

        [HttpPut("{employeeID}")]
        public async Task<IActionResult> EditEmployee(long employeeID, [FromBody] CreatedEmployeeVM editedEmployee)
        {
            var existingEmployee = await _dbContext
                .Employees
                .Include(x => x.Post)
                .FirstOrDefaultAsync(x => x.EmployeeID == employeeID && x.IsDeleted == false);

            if (existingEmployee == null)
            {
                return NotFound("Сотрудник не существует");
            }

            if (editedEmployee.FirstName.Length > 127)
            {
                return BadRequest("Имя не может содержать больше 127 символов");
            }
            if (editedEmployee.LastName.Length > 127)
            {
                return BadRequest("Фамилия не может содержать больше 127 символов");
            }
            if (editedEmployee.Patronymic?.Length > 127)
            {
                return BadRequest("Отчество не может содержать больше 127 символов");
            }
            if (editedEmployee.Phone.Length > 15)
            {
                return BadRequest("Номер телефона не может содержать больше 15 символов");
            }
            var existingPost = await _dbContext.Posts.Where(x => x.PostID == editedEmployee.PostID).FirstOrDefaultAsync();

            if (existingPost == null)
            {
                return NotFound("Должность не найдена");
            }

            existingEmployee.FirstName = editedEmployee.FirstName;
            existingEmployee.LastName = editedEmployee.LastName;
            existingEmployee.Patronymic = editedEmployee.Patronymic;
            editedEmployee.PostID = editedEmployee.PostID;
            editedEmployee.Phone = editedEmployee.Phone;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{employeeID}")]
        public async Task<IActionResult> DeleteEmployee(long employeeID)
        {
            var existingEmployee = await _dbContext
                .Employees
                .FirstOrDefaultAsync(x => x.EmployeeID == employeeID && x.IsDeleted == false);

            if (existingEmployee == null)
            {
                return NotFound("Сотрудник не найден");
            }

            existingEmployee.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{eventID}/{employeeID}")]
        public async Task<ActionResult<EmployeeCard>> GetParticipantCard(long eventID, long employeeID)
        {
            var existingEvent = await _dbContext
                .Events
                .Include(x => x.ExpectedEmployees)
                .ThenInclude(x => x.Biometrics)
                .Include(x => x.ExpectedEmployees)
                .ThenInclude(x => x.Post)
                .AsNoTracking()
                .FirstAsync(x => x.EventID == eventID);

            if (existingEvent == null)
            {
                return NotFound("Мероприятие не найдено.");
            }

            var presentEmployee = await _dbContext
                .EmployeeMarks
                .Include(x => x.Employee)
                .ThenInclude(y => y.Biometrics)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Post)
                .Where(x => x.EventID == eventID && x.EmployeeID == employeeID)
                .ToListAsync();

            if (presentEmployee.Count == 0)
            {
                return NotFound("Сотрудник не найден.");
            }

            EmployeeCard employeeCard = new();

            var employee = presentEmployee.Select(x => x.Employee).First();
            EmployeeVM employeeVM = new()
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                Patronymic = employee.Patronymic,
                Post = employee.Post.Name,
                Avatar = employee.Biometrics.Count == 0 ? null : employee.Biometrics.Select(x => x.FileID)?.First(),
            };

            if (existingEvent.ExpectedEmployees.Select(x => x.EmployeeID).Contains(employee.EmployeeID))
            {
                employeeCard.IsPresent = true;
            }
            else
            {
                employeeCard.IsPresent = false;
            }

            employeeCard.Employee = employeeVM;
            employeeCard.VideoMarks = presentEmployee.Select(x => x.VideoFileMark).ToList();

            return Ok(employeeCard);
        }

        [HttpGet("{eventID}/{uregisterPerson}")]
        public async Task<ActionResult<UnknownVisitorCard>> GetUnregisterPersonCard(long eventID, long visitorID)
        {
            var existingUnregisterPerson = await _dbContext
                .UnregisterPersonMarks
                .Include(x=>x.VideoFragment)
                .Where(x => x.EventID == eventID && x.UnregisterPersonID == visitorID)
                .ToListAsync();

            if (existingUnregisterPerson.Count == 0)
            {
                return NotFound("Незарегистрированный пользователь не найден");
            }

            UnknownVisitorCard unknownVisitorCard = new()
            {
                UnknownVisitorID = eventID
            };
            unknownVisitorCard.VideoFileMarks = existingUnregisterPerson.Select(x => new UnknownVisitorVideoFileMarks()
            {
                Mark = x.VideoFileMark,
                PhotoID = x.VideoFragment.FileID
            }).ToList();

            return Ok(unknownVisitorCard);
        } 
    }
}
