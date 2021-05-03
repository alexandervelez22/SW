using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {

        private readonly DataContext _context;

        public AccountController(DataContext context)
        {

             _context = context;

        }

        [HttpPost("register")]

        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO)
        {
            if(await UserExists(registerDTO.Username)) return BadRequest("Username ya existe");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;

        }

        private Task<bool> UserExists(object username)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> Usernamer(string username)
        {
            return await _context.Users.AnyAsync(variable => variable.UserName == username.ToLower());
        }
    }
    
}