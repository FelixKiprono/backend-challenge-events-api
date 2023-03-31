using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demoapp.Models;

namespace demoapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendingController : ControllerBase
    {
        private readonly EventDBContenxt _context;

        public AttendingController(EventDBContenxt context)
        {
            _context = context;
        }

      

        // PUT: api/AcceptInvite/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{userId} {eventId}")]
        public async Task<IActionResult> PutAttending(int userId, int eventId)
        {
            var invites = await _context.Attending.Where(att =>  att.EventId == eventId && att.UserId == userId ).FirstOrDefaultAsync();

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
                if (!AttendingExistsByUserIdAndEventId(userId,userId))
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

        // POST: api/Invite
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostAttending(List<int> userids,int eventid)
        {
          if (_context.Attending == null)
          {
              return Problem("Entity set 'EventDBContenxt.Attending'  is null.");
          }
            foreach(int id in userids)
            {
                var invite = new Attending()
                {
                     EventId = eventid,
                     UserId = id,
                     IsAttending= false


                };
                _context.Attending.Add(invite);
            }
          
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendingExists(int id,int userId)
        {
            return (_context.Attending?.Any(e => e.Id == id )).GetValueOrDefault();
        }

        private bool AttendingExistsByUserIdAndEventId(int id, int userId)
        {
            return (_context.Attending?.Any(e => e.Id == id && e.UserId == userId)).GetValueOrDefault();
        }
    }
}
