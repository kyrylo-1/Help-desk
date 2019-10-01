using AutoMapper;
using HelpDesk.API.Data;
using HelpDesk.API.Dtos;
using HelpDesk.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace HelpDesk.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IHelpDeskRepository repo;
        private readonly IMapper mapper;

        public TicketsController(IHelpDeskRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }


        [HttpGet("{id}", Name = "GetTicket")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticketFromRepo = await repo.GetTicket(id);

            var ticket = mapper.Map<TicketForReturnDto>(ticketFromRepo);

            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets(int userId)
        {
            IEnumerable<Ticket> allTickets = await repo.GetAllTickets(userId);

            return Ok(allTickets);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(int userId, [FromBody]TicketForCreationDto ticketForCreationDto)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            User userFromRepo = await repo.GetUser(userId);

            Ticket ticket = mapper.Map<Ticket>(ticketForCreationDto);
            if (userFromRepo.Tickets == null)
                userFromRepo.Tickets = new List<Ticket>();

            userFromRepo.Tickets.Add(ticket);

            if (await repo.SaveAll())
            {
                var ticketoReturn = mapper.Map<TicketForReturnDto>(ticket);
                return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticketoReturn);
            }

            return BadRequest("Could not add the ticket");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            var user = await repo.GetUser(userId);

            if (!user.Tickets.Any(p => p.Id == id))
                return Unauthorized();

            var ticketFromRepo = await repo.GetTicket(id);

            repo.Delete(ticketFromRepo);

            if (await repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }

        private bool IsUserAuthorized(int userId)
        {
             return   userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
