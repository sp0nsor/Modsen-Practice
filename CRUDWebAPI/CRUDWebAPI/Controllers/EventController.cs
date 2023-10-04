using AutoMapper;
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
        private readonly IMapper _mapper;
        private EventDto _eventDto;

        public EventController(EventContext eventContext, IMapper mapper)
        {
            _eventContext = eventContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            if (_eventContext.Events == null)
            {
                return NotFound();
            }
            var events = await _eventContext.Events.ToListAsync();
            var eventDtos = _mapper.Map<List<Event>, List<EventDto>>(events);
            return eventDtos;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            if (_eventContext.Events == null)
            {
                return NotFound();
            }

            var _event = await _eventContext.Events.FindAsync(id);

            _eventDto = _mapper.Map<Event, EventDto>(_event);

            if (_event == null)
            {
                return NotFound();
            }

            return _eventDto;
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> AddEvent(EventDto newEvent)
        {

            _eventContext.Events.Add(_mapper.Map<EventDto,Event>(newEvent));
            await _eventContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Putevent(int id, EventDto updateEvent)
        {

            var existingevent = await _eventContext.Events.FindAsync(id);
            if (existingevent == null)
            {
                return NotFound();
            }

            _mapper.Map<EventDto, Event>(updateEvent, existingevent);

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
