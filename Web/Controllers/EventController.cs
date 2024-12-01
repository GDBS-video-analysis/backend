using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Web.DataBaseContext;
using Web.DTOs;
using Web.Entities;

namespace Web.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private VideoAnalisysDBContext _dbContext;
        public EventsController(VideoAnalisysDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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

            _dbContext.Events.Update(editedEvent);
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

        [HttpPost("${eventID}/videoFile")]
        public async Task<IActionResult> AddVideoFileToEvent(long eventID, long videoFileID)
        {
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

            var existingVideo = await _dbContext
                .Files
                .Where(x => x.FileID == videoFileID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (existingVideo == null)
            {
                return NotFound("Видеозапись не найдена");
            }

            existingEvent.VideoFile = existingVideo;

            _dbContext.Events.Update(existingEvent);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("${eventID}/expectedEmployees")]
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
                    .AsNoTracking()
                    .ToListAsync();

                if (!employees.All(x => existingEmployees.Select(y => y.EmployeeID).Contains(x)))
                {
                    return NotFound("Не все работники существуют");
                }

                //var deletedEmployees = existingEvent
                //    .ExpectedEmployees
                //    .Select(x => x.EmployeeID)
                //    .Except(employees);

                //var addedEmployees = employees
                //    .Except(existingEvent
                //    .ExpectedEmployees
                //    .Select(x => x.EmployeeID));

                //var a = existingEvent.ExpectedEmployees.Select(x => x.EmployeeID).ToList().Remove(deletedEmployees);

                List<Employee> newExpectedEmployees = new();
                foreach (var employee in employees)
                {
                    newExpectedEmployees.Add(existingEmployees.Where(x => x.EmployeeID == employee).First());
                }

                existingEvent.ExpectedEmployees = newExpectedEmployees;
            }
            else
            {
                existingEvent.ExpectedEmployees.Clear();
            }

            _dbContext.Events.Update(existingEvent);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }
    }
}
