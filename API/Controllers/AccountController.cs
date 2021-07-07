using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entites;
using API.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        public AccountController (DataContext context, ITokenService tokenService)
        {
            _context=context;
            _tokenService=tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) //ActionResult keyword can allow multiple Http Status code to be returned for example: Bad Request
        {

                if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken"); //return Http 400
            

            using var hmac=new HMACSHA512(); //we use keyword using so that the function for HMACSHA512 can get released after assigning 

            var user=new AppUser
            {
                UserName=registerDto.UserName.ToLower(),
                Passwordhash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt=hmac.Key
            };

            _context.Users.Add(user);   

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            };     
        }
     

        private async Task<bool> UserExists(string userName)
        {

          return await _context.Users.AnyAsync(x=>x.UserName==userName.ToLower());
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto logindto)
        {
             var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName==logindto.UserName);

           if (user==null) return Unauthorized("Invalid username");
   
           using var hmac=new HMACSHA512(user.PasswordSalt);

           var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

            for (int i=0;i<computedHash.Length; i++)
            {
                if (computedHash[i] !=user.Passwordhash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            }; 
        }


    }


   

}