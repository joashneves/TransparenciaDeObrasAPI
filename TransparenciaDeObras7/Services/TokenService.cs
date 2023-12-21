using Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TransparenciaDeObras7.Services
{
    public class TokenService
    {
        public static object GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            var tokenCofing = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userID", user.Id.ToString()),
                }),
                Expires = DateTime.MaxValue,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenCofing);
            var tokenString = tokenHandler.WriteToken(token);

            return new { token = tokenString };

        }
    }
}
