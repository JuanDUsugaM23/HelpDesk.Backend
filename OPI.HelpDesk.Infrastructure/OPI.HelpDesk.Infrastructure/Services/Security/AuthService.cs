using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OPI.HelpDesk.Application.Interfaces.Security;
using OPI.HelpDessk.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OPI.HelpDesk.Infrastructure.Services.Security
{
    public class AuthService(IConfiguration config) : IAuthService
    {
        private const int WorkFactor = 12;
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (!string.IsNullOrEmpty(user.Role.ToString()))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: config["JwtSettings:Issuer"],
                    audience: config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(config["JwtSettings:DurationInMinutes"]!)),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public RefreshToken GenerateRefreshToken(Guid userId)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = userId,
                IsRevoked = false
            };
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
