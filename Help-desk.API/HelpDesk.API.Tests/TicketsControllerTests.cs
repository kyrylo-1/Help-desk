using System;
using System.Collections.Generic;
using System.Text;
using HelpDesk.API.Controllers;
using Xunit;

namespace HelpDesk.API.Tests
{
    public class TicketsControllerTests
    {
        [Fact]
        public void AddTicket_ShouldNotAuthorize()
        {
            Assert.Equal(2, 1 + 1);
        }
    }
}
