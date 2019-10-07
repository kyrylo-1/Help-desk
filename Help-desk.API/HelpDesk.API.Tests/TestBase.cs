using HelpDesk.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.API.Tests
{
    public abstract class TestsBase : IDisposable
    {
        protected DataContext context;
        protected TestsBase()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseSqlite("Data Source=HelpDeskTest.db");

            context = new DataContext(dbContextOptions.Options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }
    }
}
