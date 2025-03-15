using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserService.Models;

namespace UserService.Helpers
{
    public class JwtService
    {
        private readonly string _secret;
        public JwtService(string secret)
        {
            _secret = secret;
        }

        public string GenerateToken(User user, List<string> roles, List<string> permissions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())};
            claims.AddRange(roles.Select(role => new Claim("Role", role)));
            claims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
