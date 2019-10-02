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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHelpDeskRepository repo;
        private readonly IMapper mapper;

        public UserController(IHelpDeskRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            string idFromClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(idFromClaim, out int userId))
                return BadRequest("Can not parse user id");

            User userFromRepo = await repo.GetUser(userId);
            if (userFromRepo == null)
                return BadRequest(string.Format("Ticket with id {0} does not exist", userId));

            var userToReturn = mapper.Map<UserForReturnDto>(userFromRepo);

            return Ok(userToReturn);
        }
        [HttpGet("tickets/{id}", Name = "GetTicket")]
        public async Task<IActionResult> GetTicket(int id)
        {
            string userIdFromClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userIdFromClaim, out int userId))
                return BadRequest("Can not parse user id");

            User userFromRepo = await repo.GetUser(userId);

            Ticket ticketFromRepo = userFromRepo.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticketFromRepo == null)
                return BadRequest(string.Format("Ticket with id {0} does not exist", id));

            if (!IsTeamMemeber(userFromRepo.Type) && ticketFromRepo.UserId != userId)
                return Unauthorized();

            var ticket = mapper.Map<TicketForReturnDto>(ticketFromRepo);

            return Ok(ticket);
        }

        // ACCESS: TeamMember
        [HttpGet("tickets")]
        public async Task<IActionResult> GetAllTickets()
        {
            string userIdFromClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userIdFromClaim, out int userId))
                return BadRequest("Can not parse user id");

            User userFromRepo = await repo.GetUser(userId);
            IEnumerable<Ticket> allTickets;
            if (IsTeamMemeber(userFromRepo.Type))
            {
                allTickets = await repo.GetAllTickets();
            }
            else
            {
                allTickets = userFromRepo.Tickets;
            }

            if (allTickets == null)
                return BadRequest(string.Format("Failed to get all tickets for user with id {0}", userId));

            var ticketToReturn = mapper.Map<IEnumerable<TicketForReturnDto>>(allTickets);

            return Ok(ticketToReturn);
        }

        // ACCESS: HelpDesk
        [HttpPost("tickets")]
        public async Task<IActionResult> AddTicket([FromBody]TicketForCreationDto ticketForCreationDto)
        {
            string userIdFromClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userIdFromClaim, out int userId))
                return BadRequest("Can not parse user id");

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


        [HttpPatch("tickets/{id}")]
        public async Task<IActionResult> PatchTicket(int id, [FromBody]TicketForUpdateDto ticketForUpdateDto)
        {
            string userIdFromClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userIdFromClaim, out int userId))
                return BadRequest("Can not parse user id");

            User userFromRepo = await repo.GetUser(userId);
            Ticket ticketFromRepo;
            if (IsTeamMemeber(userFromRepo.Type))
            {
                ticketFromRepo = await repo.GetTicket(id);
            }
            else
            {
                ticketFromRepo = userFromRepo.Tickets.FirstOrDefault(t => t.Id == id);
            }

            if (ticketFromRepo == null)
                return BadRequest(string.Format("Ticket with id {0} does not exist", id));

            ticketFromRepo.Description = ticketForUpdateDto.Description;
            repo.Update(ticketFromRepo);

            if (await repo.SaveAll())
            {
                var ticket = mapper.Map<TicketForReturnDto>(ticketFromRepo);
                return Ok(ticket);
            }

            return BadRequest("Failed to update the ticket");
        }

        // ACCESS: TeamMember
        [HttpDelete("tickets/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            string userIdFromClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(userIdFromClaim, out int userId))
                return BadRequest("Can not parse user id");

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
        private bool IsClaimsIdAndRouteIdSame(int userId)
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
