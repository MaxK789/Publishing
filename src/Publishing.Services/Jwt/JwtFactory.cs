using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

namespace Publishing.Services;

public class JwtFactory : IJwtFactory
{
    private readonly IConfiguration _configuration;

    public JwtFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(UserDto user)
    {
        var issuer = _configuration["JWT:Issuer"] ?? "example.com";
        var audience = _configuration["JWT:Audience"] ?? "example.com";
        var signingKey = _configuration["JWT:SigningKey"] ?? "MySuperSecretKey1234567890123456";

        var claims = new[]
        {
            new Claim("sub", user.Id),
            new Claim("name", user.Name),
            new Claim("role", user.Type)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims,
            expires: DateTime.UtcNow.AddMinutes(30), signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
