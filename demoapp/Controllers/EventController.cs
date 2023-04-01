using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demoapp.Models;
using System.Net.Http;
using demoapp.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demoapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventDBContenxt _context;
        private readonly UserService _userService;

        public EventController(EventDBContenxt context)
        {
            _context =  context;
            _userService = new UserService();

        }

        // GET: api/Events
        [HttpGet("All/{page:int} {resultsPerPage:float}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents(int page,float resultsPerPage)
        {
          if (_context.Event == null)
          {
              return NotFound();
          }
            var itemsPerPage = (float)resultsPerPage;
            //fetch all events
            var events = await _context.Event
             .ToListAsync();
            //paginated
           var paginatedEvents =  events
                .Skip((page - 1) * (int)itemsPerPage)
                .Take((int)itemsPerPage);

            var pageCount = Math.Ceiling(events.Count() / itemsPerPage);

            //custom resonse to include page metadata 
            var response =  new
            {
                Events = paginatedEvents,
                CurrentPage = page,
                TotalPages = (int)pageCount,
                Message = "Success",
                Error = "",


            };
            return Ok(response);
        }


        //// GET: api/Event/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            User user = await this._userService.GetUser(1);
            Console.Write(user);
            if (_context.Event == null)
            {
                return NotFound();
            }
            var @event = await _context.Event.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }
            //attach user information to the response
            var customEvent =  new Event { Id = @event.Id, Title = @event.Title, Description = @event.Description, StartDate = @event.StartDate, EndDate = @event.EndDate, TimeZone = @event.TimeZone, User = user };

            var response = new
            {
                Data = customEvent,
                Message = "Success",
                Error = "",
            };
            return Ok(response);
        }
        

        // PUT: api/Event/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var response = new
            {
                Data = @event,
                Message = "Success",
                Error = "",

            };
            return Ok(response);
        }

        // POST: api/Event
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
          if (_context.Event == null)
          {
              return Problem("Entity set 'EventDBContenxt.Event'  is null.");
          }
            _context.Event.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Event/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (_context.Event == null)
            {
                return NotFound();
            }
            //
            var usedEvents = await _context.Attending.Where(item=>item.EventId==id).ToListAsync();
            if (usedEvents.Count>0)
            {
                var result = new
                {
                    Data = usedEvents,
                    Message = "Cant Delete Event,its active on Attendance!",
                    Error = "",

                };
                return Ok(result);

            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();

            var response = new
            {
                Data = "",
                Message = "Success",
                Error = "",

            };
            return Ok(response);
        }

        private bool EventExists(int id)
        {
            return (_context.Event?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
