using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerApp.Infrastructure.JwtTokenGenerators;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public string GenerateToken(CustomerId customerId, FirstName firstName, LastName lastName)
    {
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            algorithm: SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, customerId.Value.ToString()),
                new(JwtRegisteredClaimNames.GivenName, firstName.Value),
                new(JwtRegisteredClaimNames.FamilyName, lastName.Value),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
