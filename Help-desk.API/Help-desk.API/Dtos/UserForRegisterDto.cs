using HelpDesk.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
