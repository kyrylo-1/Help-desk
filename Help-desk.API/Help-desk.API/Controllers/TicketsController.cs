using AutoMapper;
using HelpDesk.API.Data;
using HelpDesk.API.Dtos;
using HelpDesk.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllTickets (int id)
        {
            var tickets = await repo.GetAllTickets(id);


            return Ok(tickets);
        }

        [HttpGet("{id}", Name = "GetTicket")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticketFromRepo = await repo.GetTicket(id);

            var ticket = mapper.Map<TicketForReturnDto>(ticketFromRepo);

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(int userId, [FromBody]TicketForCreationDto ticketForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await repo.GetUser(userId);
            //userFromRepo.Tickets.Add(ticketForCreationDto);
            //return Ok(ticket);

            Ticket ticket = mapper.Map<Ticket>(ticketForCreationDto);
            if (userFromRepo.Tickets == null)
                userFromRepo.Tickets = new List<Ticket>();

            userFromRepo.Tickets.Add(ticket);

            if (await repo.SaveAll())
            {
                var ticketoReturn = mapper.Map<TicketForReturnDto>(ticket);
                return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticketoReturn);
                //return Ok(new
                //{
                //    ticket
                //});
            }

            return BadRequest("Could not add the ticket");
        }
    }
}
