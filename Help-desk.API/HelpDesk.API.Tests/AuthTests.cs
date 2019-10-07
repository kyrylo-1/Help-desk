using System;
using HelpDesk.API.Data;
using HelpDesk.API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HelpDesk.API.Services;
using System.Security.Cryptography;
using System.Text;

namespace HelpDesk.API.Tests
{
    public class AuthTests: TestsBase
    {
        private User GetSampleUser
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
        [Fact]
        public async void Register_ShouldRegister()
        {
            AuthRepository authRepo = new AuthRepository(context);
            User userToCreate = GetSampleUser;

            string inputedPass = "password";
            User createdUser = await authRepo.Register(userToCreate, inputedPass);
            Assert.NotNull(createdUser);
            Assert.Equal(userToCreate.Username, createdUser.Username);
            Assert.True(createdUser.Id > 0);

            Assert.NotNull(createdUser.PasswordHash);
            Assert.NotEmpty(createdUser.PasswordHash);

            Assert.NotNull(createdUser.PasswordSalt);
            Assert.NotEmpty(createdUser.PasswordSalt);

            Assert.True(PasswordUtil.VerifyPasswordHash(inputedPass, createdUser.PasswordHash, createdUser.PasswordSalt));
        }

        [Fact]
        public async void Register_NotPass_EmptyPass()
        {
            AuthRepository authRepo = new AuthRepository(context);
            User userToCreate = GetSampleUser;

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => authRepo.Register(userToCreate, ""));
            Assert.Equal("Parameters are invalid", ex.Message);
        }

        [Fact]
        public async void Register_NotPass_NullUser()
        {
            AuthRepository authRepo = new AuthRepository(context);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => authRepo.Register(null, "pass"));
            Assert.Equal("Parameters are invalid", ex.Message);
        }
    }
}
