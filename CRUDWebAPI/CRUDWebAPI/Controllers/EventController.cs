using CRUDWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private EventContext _eventContext;

        public EventController(EventContext eventContext)
        {
            _eventContext = eventContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            if (_eventContext.Events == null)
            {
                return NotFound();
            }
            return await _eventContext.Events.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            if (_eventContext.Events == null)
            {
                return NotFound();
            }

            var _event = await _eventContext.Events.FindAsync(id);

            if (_event == null)
            {
                return NotFound();
            }

            return _event;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> AddEvent(Event newEvent)
        {
            _eventContext.Events.Add(newEvent);
            await _eventContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new {id = newEvent.Id}, newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Putevent(int id, Event updateEvent)
        {
            if (id != updateEvent.Id)
            {
                return BadRequest();
            }

            var existingevent = await _eventContext.Events.FindAsync(id);
            if (existingevent == null)
            {
                return NotFound();
            }

            existingevent.Name = updateEvent.Name;
            existingevent.Description = updateEvent.Description;
            existingevent.Speaker = updateEvent.Speaker;
            existingevent.Time = updateEvent.Time;
            existingevent.Place = updateEvent.Place;

            try
            {
                await _eventContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();

        }
        private bool BrandAvailable(int id)
        {
            return (_eventContext.Events?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (_eventContext.Events == null)
            {
                return NotFound();
            }

            var _event = await _eventContext.Events.FindAsync(id);
            if (_event == null)
            {
                return NotFound();
            }
            _eventContext.Events.Remove(_event);
            await _eventContext.SaveChangesAsync();

            return Ok();
        }
    }
}
