using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiRest.Helper
{
    public class JWTHelper
    {
        private readonly byte[] _secret;
        public JWTHelper(string secretKey)
        {
            this._secret = Encoding.ASCII.GetBytes(secretKey);
        }

        #region El método para crear el token
        public string CreateToken(string @username)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, @username));

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(this._secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createToken = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(createToken);
        }
        #endregion
    }
}
