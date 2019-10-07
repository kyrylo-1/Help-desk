using HelpDesk.API.Data;
using HelpDesk.API.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.API.Tests
{
    public class TestsBase : IDisposable
    {
        protected readonly  DataContext context;
        protected readonly SqliteConnection connection;
        public User GetSampleUser
        {
            get
            {
                return new User
                {
                    Username = "user-test1",
                    Type = "HelpDeskUser"
                };

            }
        }
 
        public TestsBase()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
                            .UseSqlite(connection)
                            .Options;

            context = new DataContext(options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
