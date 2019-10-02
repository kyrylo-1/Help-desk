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
                return BadRequest("Can not parse id");

            User userFromRepo = await repo.GetUser(userId);
            if (userFromRepo == null)
                return BadRequest(string.Format("Ticket with id {0} does not exist", userId));


            IEnumerable<Ticket> allTickets;
            if (IsTeamMemeber(userFromRepo.Type))
                allTickets = await repo.GetAllTickets();
            
            else
                allTickets = userFromRepo.Tickets;            

            if (allTickets == null)
                return BadRequest(string.Format("Failed to get all tickets for user with id {0}", userId));

            var ticketsToReturn = mapper.Map<IEnumerable<TicketForReturnDto>>(allTickets);

            return Ok(new
            {
                username = userFromRepo.Username,
                tickets = ticketsToReturn
            });
        }
        private bool IsTeamMemeber(string userType)
        {
            return string.Equals(userType, UserType.TeamMember.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
