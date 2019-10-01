using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.API.Dtos
{
    public class TicketForCreationDto
    {
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public TicketForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}
