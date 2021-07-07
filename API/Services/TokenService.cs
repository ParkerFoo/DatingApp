using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entites;
using API.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key; //symmetricsecurity key. meaning we will use symetric encryption aka public encryption. Means only one key will be used since the key will not leave the server.
        public TokenService(IConfiguration config)
        {
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
           var claims = new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
           };

           var creds=new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

           var tokenDescriptor = new SecurityTokenDescriptor
           { 
               Subject= new ClaimsIdentity(claims),
               Expires=DateTime.Now.AddDays(7),
               SigningCredentials=creds
           };

           var tokenHandler=new JwtSecurityTokenHandler();
           var token=tokenHandler.CreateToken(tokenDescriptor);
           return tokenHandler.WriteToken(token);

        }
    }
}