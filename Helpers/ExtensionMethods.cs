using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CustomTrackerBackend.Models;
using CustomTrackerBackend.Models.Inputs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace CustomTrackerBackend.Helpers
{
    public static class ExtensionMethods
    {
        public static async Task<User> CreateUserAsync(this UserContext context, LoginInput input, string password)
        {
            string passwordHash = password.HashPassword();
            User fullUser = new User()
            {
                Username = input.Username,
                PasswordHash = passwordHash
            };
            await context.AddAsync(fullUser);
            await context.SaveChangesAsync();
            return fullUser;
        }

        private static string HashPassword(this string password)
        {
            var salt = new byte[128 / 8];
            using(var rng = RandomNumberGenerator.Create()) { rng.GetBytes(salt); }
            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 12,
                numBytesRequested: 256 / 8
            ));
            return passwordHash;
        }

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
    }
}
