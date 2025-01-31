using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EMPLOYEE_INFORMATION.Helpers
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string userId, string role)
        {
            var jwtSettings = _configuration.GetSection("jwtSettings");
            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiry = Convert.ToInt16(jwtSettings["ExpiryInMinutes"]);

            var claim = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userId),
                 new Claim(ClaimTypes.Role, role),
                 new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddMinutes(expiry),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = credentials,
                

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(token);
        }
    }
}
