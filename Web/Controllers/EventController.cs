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

        public async Task<IActionResult<>>


        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent([FromBody] CreatedEventVM eventVM)
        {
            if(eventVM.Name.Length > 127)
            {
                return BadRequest("Название мероприятия не может быть длинее 127 символов.");
            }

            var dbEvents = await _dbContext.Events.ToListAsync();

            Event NewEvent = new()
            {
                Name = name,
                DateTime = eventDateTime.ToUniversalTime(),
                Description = description
            };

            _dbContext.Events.Add(NewEvent);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [GeneratedRegex(@"^[a-zA-Zа-яА-Я0-9][a-zA-Zа-яА-Я0-9\s\p{P}]+$")]
        private static partial Regex ValidationExpression();
    }
}
