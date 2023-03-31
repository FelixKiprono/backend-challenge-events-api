using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demoapp.Models;
using demoapp.Services;
using System.Net;

namespace demoapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly EventDBContenxt _context;
        private readonly UserService _userService;

        public InvitationController(EventDBContenxt context)
        {
            _context = context;
            _userService = new UserService();

        }

        // GET: api/Invitation/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<dynamic>> GetEvent(int id)
        {
            User user = await this._userService.GetUser(id);
            if (_context.Attending == null)
            {
                return NotFound();
            }

            var invitations = (from invites in _context.Attending
                               join events in _context.Event
                               on invites.EventId equals events.Id
                               where invites.UserId == id && invites.IsAttending == false
                               select new
                               {
                                   Title = events.Title,
                                   Description = events.Description,
                                   StartDate = events.StartDate,
                                   EndDate = events.EndDate,
                                   TimeZone = events.TimeZone,
                                   IsAttending = invites.IsAttending,
                                   TimeStamp = invites.TimeStamp,
                                   User = user,

                               }).ToList();

            var @event = await _context.Event.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }
            return invitations;
        }
        // PUT: api/AcceptInvitation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Accept/{invitationId} {userId} {eventId} ")]
        public async Task<IActionResult> PutAttending(int invitationId,int userId, int eventId)
        {
            var invites = await _context.Attending.Where(att => att.EventId == eventId && att.UserId == userId && att.Id==invitationId).FirstOrDefaultAsync();

            if (invites == null)
            {
                return NotFound();
            }

            try
            {
                invites.IsAttending = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendingExistsByUserIdAndEventId(userId, userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SendInvitation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Send/{eventId} {userIds}")]
        public async Task<ActionResult> PostAttending(int eventId, List<int> userIds)
        {
          if (_context.Attending == null)
          {
              return Problem("Entity set 'EventDBContenxt.Attending'  is null.");
          }
            if (!this.EventExist(eventId))
            {
                return Problem("Entity set 'EventDBContenxt.Event'  is null or not available.");
            }

            foreach (int id in userIds)
            {
                var invite = new Attending()
                {
                     EventId = eventId,
                     UserId = id,
                     IsAttending= false


                };
                _context.Attending.Add(invite);
            }
          
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        private bool AttendingExistsByUserIdAndEventId(int id, int userId)
        {
            return (_context.Attending?.Any(e => e.Id == id && e.UserId == userId)).GetValueOrDefault();
        }

        private bool EventExist(int id)
        {
            return (_context.Event?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
