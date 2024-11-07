using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service;

public static class TokenService
{
    public static string GenerateToken(string secretJWT, string credential, string idUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[]? key = Encoding.ASCII.GetBytes(secretJWT);

        JwtSecurityToken? token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, credential),
                new Claim(ClaimTypes.NameIdentifier, idUser)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            
        });

        return tokenHandler.WriteToken(token);
    }
}
