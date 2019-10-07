using HelpDesk.API.Data;
using HelpDesk.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HelpDesk.API.Tests
{
    public class UserTests : TestsBase
    {
        public const string SAMPLE_PASS = "password";

        [Fact]
        public async void Patch_ShouldPass()
        {
            User userToCreate = new User
            {
                Username = "user-test3",
                Type = "HelpDeskUser"
            };

            User createdUser = await new AuthRepository(context).Register(userToCreate, "password");

            var helpDeskRepo = new HelpDeskRepository(context);

            string firstTicketDescription = "Some Description";
            Ticket ticket = new Ticket
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Description = firstTicketDescription,
                UserId = createdUser.Id,
                User = createdUser
            };
            await helpDeskRepo.AddTicket(createdUser, ticket);

            string patchedDescription = ticket.Description + "update";
            Ticket patchedTicket = await helpDeskRepo.PatchTicket(ticket.UserId, ticket.Id, patchedDescription);

            Assert.NotNull(patchedTicket);
            Assert.Equal(patchedTicket.Id, ticket.Id);
            Assert.Equal(patchedTicket.UserId, ticket.UserId);
            Assert.Equal(patchedTicket.DateAdded, ticket.DateAdded);

            Assert.NotEqual(patchedTicket.Description, firstTicketDescription);
            Assert.Equal(patchedTicket.Description, patchedDescription);
        }

        [Fact]
        public async void Patch_Should_Not_Pass()
        {
            //id -1 can not exist
            Ticket patchedTicket = await new HelpDeskRepository(context).PatchTicket(-1, 1, "");
            Assert.Null(patchedTicket);
        }
    }
}
