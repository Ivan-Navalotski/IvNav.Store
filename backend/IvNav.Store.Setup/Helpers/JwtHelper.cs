using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace IvNav.Store.Setup.Helpers;

public class JwtHelper
{
    private readonly string _secretKey;
    private readonly TimeSpan _expires;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtHelper(IConfiguration configuration)
    {
        var section = Guard.Against.Null(configuration.GetSection("Authentication:Jwt"));
            
        _secretKey = section.GetValue<string>("Secret")!;
        _expires = section.GetValue<TimeSpan>("Expires");
        _issuer = section.GetValue<string>("Issuer")!;
        _audience = section.GetValue<string>("Audience")!;
    }

    public string Generate(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_expires),
            SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = GetKey()
        };
    }

    private SymmetricSecurityKey GetKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    }
}
