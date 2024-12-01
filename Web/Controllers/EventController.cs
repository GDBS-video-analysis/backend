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
    public partial class EventsController : ControllerBase
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

            return eventsVM;
        }

        [HttpPost("event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreatedEventVM eventVM)
        {
            if(eventVM.Name.Length > 127)
            {
                return BadRequest("Название мероприятия не может быть длинее 127 символов.");
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

        [GeneratedRegex(@"^[a-zA-Zа-яА-Я0-9][a-zA-Zа-яА-Я0-9\s\p{P}]+$")]
        private static partial Regex ValidationExpression();
    }
}
