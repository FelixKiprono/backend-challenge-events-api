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
using System.ComponentModel;

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
        public async Task<ActionResult<dynamic>> GetUserInvitation(int id)
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
                                   Id = invites.Id,
                                   Title = events.Title,
                                   IsAttending = invites.IsAttending,
                                   Notes = invites.Notes,
                                   TimeStamp = invites.TimeStamp,
                                   EventsId = events.Id,
                                   Description = events.Description,
                                   StartDate = events.StartDate,
                                   EndDate = events.EndDate,
                                   TimeZone = events.TimeZone,
                                   User = user,

                               }).ToList();

            var response = new
            {
                Data = invitations,
                Message = invitations.Count>0? "Success" : "No Invites Currently",
                Error = "",

            };
            return Ok(response);
        }
        // PUT: api/Accept/{invitationId} {userId} {eventId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Accept/")]
        public async Task<IActionResult> AcceptInvitation(int invitationId,int userId, int eventId)
        {
            var invites = await _context.Attending.Where(att => att.EventId == eventId && att.UserId == userId && att.Id==invitationId).FirstOrDefaultAsync();

            if (invites == null)
            {
                var res = new
                {
                    Data = invites,
                    Message = "No Invitation Found with those Parameters!",
                    Error = "",

                };
                return NotFound(res);
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

            var response = new
            {
                Data = invites,
                Message = "Successfuly Accepted attendance!",
                Error = "",

            };
            return Ok(response);

        }

        // POST: api/SendInvitation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Send/{eventId} {userIds}")]
        public async Task<ActionResult> SendInvitation(int eventId, List<int> userIds)
        {
          if (_context.Attending == null)
          {
              return Problem("Entity set 'EventDBContenxt.Attending'  is null.");
          }
            if (!this.EventExist(eventId))
            {
                return NotFound("Entity set 'EventDBContenxt.Event'  is null or not available.");
            }
         
            var invitations = new List<Attending>();
            foreach (int id in userIds)
            {
                if (!AttendingExistsByUserIdAndEventId(eventId, id))
                {
                    invitations.Add(
                     new Attending()
                     {
                         EventId = eventId,
                         UserId = id,
                         IsAttending = false
                     });
                }
            }
            _context.Attending.AddRange(invitations);

            await _context.SaveChangesAsync();

            var response = new
            {
                Data = invitations,
                Message = "Successfuly Sent attendance!",
                Error = "",

            };
            return Ok(response);
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
