using System;
using HelpDesk.API.Data;
using HelpDesk.API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HelpDesk.API.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.API.Tests
{
    public class AuthTests : TestsBase
    {
        public const string SAMPLE_PASS = "password";

        [Fact]
        public async void Register_ShouldRegister()
        {
            var authRepo = new AuthRepository(context);
            User userToCreate = GetSampleUser;
            User createdUser = await authRepo.Register(userToCreate, SAMPLE_PASS);

            CheckUser(userToCreate, createdUser, SAMPLE_PASS);
        }

        [Fact]
        public async void Register_NotPass_EmptyPass()
        {
            var authRepo = new AuthRepository(context);
            User userToCreate = GetSampleUser;

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => authRepo.Register(userToCreate, ""));
            Assert.Equal("Parameters are invalid", ex.Message);
        }

        [Fact]
        public async void Register_Not_Pass_NullUser()
        {
            var authRepo = new AuthRepository(context);

            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => authRepo.Register(null, "pass"));
            Assert.Equal("Parameters are invalid", ex.Message);
        }

        [Fact]
        public async void Login_Pass()
        {
            var authRepo = new AuthRepository(context);
            User userToCreate = GetSampleUser;

            await authRepo.Register(userToCreate, SAMPLE_PASS);

            User userLoggedIn = await authRepo.Login(userToCreate.Username, SAMPLE_PASS);
            CheckUser(userToCreate, userLoggedIn, SAMPLE_PASS);
        }

        [Fact]
        public async void Login_Not_Passed()
        {
            var authRepo = new AuthRepository(context);
            User userToCreate = new User
            {
                Username = "user-test2",
                Type = "TeamMember"
            }; ;

            User user = await authRepo.Login(userToCreate.Username, SAMPLE_PASS);
            Assert.Null(user);
        }

        private void CheckUser(User userToCreate, User createdUser, string providedPass)
        {
            Assert.NotNull(createdUser);
            Assert.Equal(userToCreate.Username, createdUser.Username);
            Assert.True(createdUser.Id > 0);

            Assert.NotNull(createdUser.PasswordHash);
            Assert.NotEmpty(createdUser.PasswordHash);

            Assert.NotNull(createdUser.PasswordSalt);
            Assert.NotEmpty(createdUser.PasswordSalt);

            Assert.True(PasswordUtil.VerifyPasswordHash(providedPass, createdUser.PasswordHash, createdUser.PasswordSalt));
        }
    }
}
