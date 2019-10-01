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
using System;

namespace HelpDesk.API.Controllers
{
    //[Route("api/users/{userId}/[controller]")]
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
        public async Task<IActionResult> GetTicket(int userId, int id)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            User userFromRepo = await repo.GetUser(userId);     

            Ticket ticketFromRepo = userFromRepo.Tickets.FirstOrDefault(t => t.Id == id);
            if(ticketFromRepo == null)
                return BadRequest("ticket doesn't exist");

            if (!IsTeamMemeber(userFromRepo.Type) && ticketFromRepo.UserId != userId)
                return Unauthorized();            

            var ticket = mapper.Map<TicketForReturnDto>(ticketFromRepo);

            return Ok(ticket);
        }

        // ACCESS: TeamMember
        [HttpGet]
        public async Task<IActionResult> GetAllTickets(int userId)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            User userFromRepo = await repo.GetUser(userId);
            if (!IsTeamMemeber(userFromRepo.Type))
                return Unauthorized();

            IEnumerable<Ticket> allTickets = await repo.GetAllTickets();
            var ticketToReturn = mapper.Map<IEnumerable<TicketForListDto>>(allTickets);

            return Ok(ticketToReturn);
        }

        // ACCESS: HelpDesk
        [HttpPost]
        public async Task<IActionResult> AddTicket(int userId, [FromBody]TicketForCreationDto ticketForCreationDto)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            User userFromRepo = await repo.GetUser(userId);

            // Only HelpDesk user can add tasks
            if (IsTeamMemeber(userFromRepo.Type))
                return Unauthorized();

            Ticket ticket = mapper.Map<Ticket>(ticketForCreationDto);
            if (userFromRepo.Tickets == null)
                userFromRepo.Tickets = new List<Ticket>();

            userFromRepo.Tickets.Add(ticket);

            if (await repo.SaveAll())
            {
                var ticketoReturn = mapper.Map<TicketForReturnDto>(ticket);
                return CreatedAtRoute("GetTicket", new { userId, id = ticket.Id }, ticketoReturn);
            }

            return BadRequest("Could not add the ticket");
        }

        // ACCESS: TeamMember
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            User userFromRepo = await repo.GetUser(userId);
            if (!IsTeamMemeber(userFromRepo.Type))
                return Unauthorized();

            Ticket ticketFromRepo = await repo.GetTicket(id);
            if (ticketFromRepo == null)
            {
                return BadRequest(string.Format("Ticket with id {0} doesn't exist", id));
            }

            repo.Delete(ticketFromRepo);

            if (await repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }

        /// <summary>
        /// Verifies that id from claim the same as id from route
        /// </summary>
        private bool IsUserAuthorized(int userId)
        {
            Claim first = User.FindFirst(ClaimTypes.NameIdentifier);
            return userId == int.Parse(first.Value);
        }

        private bool IsTeamMemeber(string userType)
        {
            return string.Equals(userType, UserType.TeamMember.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
