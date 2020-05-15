using System;
using Microsoft.Extensions.Configuration;
using CustomTrackerBackend.Models;
using CustomTrackerBackend;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;

namespace CustomTrackerBackend.Helpers
{
    public class TokenManager
    {
        public TokenManager() { }

        public static string GenerateToken(User user)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: Startup.Configuration["Host"],
                claims: claims.Claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Startup.Configuration["SiteKey"])), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}