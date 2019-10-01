using AutoMapper;
using HelpDesk.API.Dtos;
using HelpDesk.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Ticket, TicketForCreationDto>();
            CreateMap<TicketForCreationDto, Ticket>();

            CreateMap<Ticket, TicketForReturnDto>();
            CreateMap<TicketForReturnDto, Ticket>();
        }
    }
}
