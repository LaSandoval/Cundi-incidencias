using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Cundi_incidencias.Utility
{
    public class TokenUtility
    {
        private readonly IConfiguration _config;
        public TokenUtility(IConfiguration config)
        {
            _config = config;
        }
        public string GenerarToken(string CC, string rol)
        {
            var SecretKey = "castrobar2024GeNNierAlejoFlorezMateo";
            var security = Encoding.ASCII.GetBytes(SecretKey);

            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,CC),
                new Claim(ClaimTypes.Role,rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var tokenDescriptor = new JwtSecurityToken(
                    claims: claims,
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddHours(2)).DateTime,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(security), SecurityAlgorithms.HmacSha256)
                );
            //generate token and create
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            //return token to aplication
            return token;
        }
    }
}
