using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.API.Dtos
{
    public class UserForReturnDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
    }
}
