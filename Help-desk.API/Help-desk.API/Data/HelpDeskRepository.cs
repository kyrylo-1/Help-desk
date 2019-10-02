using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);

        }
        /// <summary>
        /// Get all tickets from all users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Ticket>> GetAllTickets()
        {
           var allTickets = await context.Tickets.ToListAsync();

            return context.Tickets;
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

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
