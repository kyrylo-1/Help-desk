using HelpDesk.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.API.Data
{
    public interface IHelpDeskRepository
    {
        void Delete<T>(T entity) where T : class;
        Task<Ticket> GetTicket(int id);
        Task<IEnumerable<Ticket>> GetTickets(User user);
        Task<Ticket> PatchTicket(int userId, int ticketId, string ticketDescription);
        Task<bool> AddTicket(User user, Ticket ticket);

        Task<User> GetUser(int userId);
        Task<bool> SaveAll();
    }
}
