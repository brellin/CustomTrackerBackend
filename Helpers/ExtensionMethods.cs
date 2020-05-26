using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CustomTracker.Models;
using Microsoft.IdentityModel.Tokens;

namespace CustomTracker.Helpers
{
    public static class ExtensionMethods
    {

        public static string GenerateToken(this User user)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Startup.Configuration["PG:Host"],
                claims : claims.Claims,
                notBefore : DateTime.Now,
                expires : DateTime.Now.AddDays(30),
                signingCredentials : new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Startup.Configuration["SiteKey"])), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string HashPassword(this string password)
        {
            byte[] bytes = SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int i in bytes) stringBuilder.Append(i.ToString("x2"));
            return stringBuilder.ToString();
        }
    }
}
