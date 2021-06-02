using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
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
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;

        }

        
        [HttpPost("login")]

        public async Task<ActionResult<AppUser>> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(users =>users.UserName == loginDTO.Username );
            
            if(user == null) return Unauthorized("Usuario Invalido");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for(int i = 0;i < ComputeHash.Length;i++)
            {
                if(ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Password invalido");
            }

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