using System.Collections.Generic;

namespace HelpDesk.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
