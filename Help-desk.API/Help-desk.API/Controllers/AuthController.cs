using HelpDesk.API.Data;
using HelpDesk.API.Dtos;
using HelpDesk.API.Models;
using HelpDesk.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            bool isValidUserType  = EnumHelper.DoesStringExistInEnum(typeof(UserType), userForRegisterDto.Type);
            if (!isValidUserType)
            {
                return BadRequest(string.Format("User type '{0}' is not valid",
                         userForRegisterDto.Type));
            }

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username is already exists");
            }

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username,
                Type = userForRegisterDto.Type.ToString()
            };

            User createdUser = await repo.Register(userToCreate, userForRegisterDto.Password);

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken createdToken = CreateToken(createdUser.Id.ToString(), createdUser.Username, tokenHandler);

            return StatusCode(201, new
            {
                token = tokenHandler.WriteToken(createdToken)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken createdToken = CreateToken(userFromRepo.Id.ToString(), userFromRepo.Username, tokenHandler);

            return Ok(new
            {
                token = tokenHandler.WriteToken(createdToken)
            });
        }

        private SecurityToken CreateToken(string userId, string userName, JwtSecurityTokenHandler tokenHandler)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
