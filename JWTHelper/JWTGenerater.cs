using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryManagementWithExpirationDatesSystem.JWTHelper
{
    public class JWTGenerater
    {
        private readonly IConfiguration _configuration;

        public JWTGenerater(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string JWTTokenGenerator(string username, string email = "", string role = "Admin")
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Username cannot be null or empty");

            var jwtSettings = _configuration.GetSection("JWT");

            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expireMinutes = jwtSettings["ExpireMinutes"];

            if (string.IsNullOrWhiteSpace(key) ||
                string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience) ||
                string.IsNullOrWhiteSpace(expireMinutes))
            {
                throw new InvalidOperationException("One or more JWT configuration values are missing in appsettings.json.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, "ithanan@gmail.com"),
                new Claim(ClaimTypes.Role, role),
            };

            if (!string.IsNullOrWhiteSpace(email))
                claims.Add(new Claim(ClaimTypes.Email, email));

            if (!string.IsNullOrWhiteSpace(role))
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(expireMinutes)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

