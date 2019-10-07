using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelpDesk.API.Dtos;
using HelpDesk.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Data
{
    public class HelpDeskRepository : IHelpDeskRepository
    {
        private readonly DataContext context;

        public HelpDeskRepository(DataContext context)
        {
            this.context = context;
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<bool> AddTicket(User user, Ticket ticket)
        {
            if (user.Tickets == null)
                user.Tickets = new List<Ticket>();

            user.Tickets.Add(ticket);

            return await SaveAll();
        }

        /// <summary>
        /// Get all tickets from all users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Ticket>> GetTickets(User user)
        {
            IEnumerable<Ticket> allTickets = IsTeamMemeber(user.Type) ?
                                                await context.Tickets.ToListAsync() :
                                                user.Tickets;

            return allTickets;
        }

        public async Task<Ticket> GetTicket(int id)
        {
            var ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

            return ticket;
        }

        public async Task<User> GetUser(int id)
        {
            User user = await context.Users.Include(t => t.Tickets).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Ticket> PatchTicket(int userId, int ticketId, string ticketDescription)
        {
            User userFromRepo = await GetUser(userId);
            if (userFromRepo == null)
                return null;

            Ticket ticket;
            if (IsTeamMemeber(userFromRepo.Type))
            {
                ticket = await GetTicket(ticketId);
            }
            else
            {
                ticket = userFromRepo.Tickets.FirstOrDefault(t => t.Id == ticketId);
            }

            if (ticket == null)
            {
                throw new System.ArgumentException(string.Format("Ticket with id {0} does not exist", ticketId));
            }
            ticket.Description = ticketDescription;
            context.Update(ticket);

            if (await SaveAll())
            {
               return ticket;
            }
            return null;
        }


        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }

        private bool IsTeamMemeber(string userType)
        {
            return string.Equals(userType, UserType.TeamMember.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
