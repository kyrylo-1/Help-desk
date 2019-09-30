using System.Threading.Tasks;
using HelpDesk.API.Models;
using HelpDesk.API.Services;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        private readonly PasswordService passService;
        public AuthRepository(DataContext context)
        {
            this.context = context;
            passService = new PasswordService();
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            passService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!passService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }


        public async Task<bool> UserExists(string username)
        {
            if (await context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}
