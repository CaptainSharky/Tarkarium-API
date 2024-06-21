using Microsoft.AspNetCore.Mvc;
using education.Tickets;
using System.Threading.Tasks;

namespace education.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddTicketToUserAsync(TicketEntity ticket)
        {
            bool result = await _ticketService.AddTicketToUserAsync(ticket);
            if (result)
            {
                return Ok("Ticket added to user successfully");
            }
            else
            {
                return BadRequest("Failed to add ticket to user");
            }
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicketAsync(int ticketId)
        {
            var ticket = await _ticketService.GetTicketAsync(ticketId);
            if (ticket != null)
            {
                return Ok(ticket);
            }
            else
            {
                return NotFound("Ticket not found");
            }
        }

        [HttpGet("info/{ticketId}")]
        public async Task<IActionResult> GetTicketInfoAsync(int ticketId)
        {
            var ticketInfo = await _ticketService.GetTicketInfoAsync(ticketId);
            if (ticketInfo != null)
            {
                return Ok(ticketInfo);
            }
            else
            {
                return NotFound("Ticket info not found");
            }
        }

        [HttpGet("user/{username}/{email}")]
        public async Task<IActionResult> GetUserTicketsAsync(string username, string email)
        {
            var userTickets = await _ticketService.GetUserTicketsAsync(username, email);
            if (userTickets.Count > 0)
            {
                return Ok(userTickets);
            }
            else
            {
                return NotFound("User has no tickets");
            }
        }
    }
}
