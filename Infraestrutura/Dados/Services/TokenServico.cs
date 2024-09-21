using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;

public class TokenServico : ITokenServico
{
    private readonly IConfiguration _configuration;

    public TokenServico(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CriarToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = _configuration["Token:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1), 
            Issuer = _configuration["Token:Issuer"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
