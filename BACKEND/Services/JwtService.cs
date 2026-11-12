


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly IConfiguration configuration;

    public JwtService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateAccessToken(long UserId)
    {
        string Key = configuration["JwtProperties:Key"] ?? throw new InvalidOperationException("Key not found");
        string Issuer = configuration["JwtProperties:Issuer"] ?? throw new InvalidOperationException("Issuer not found");
        string Audience = configuration["JwtProperties:Audience"] ?? throw new InvalidOperationException("Audince not found");
        int ExpiresMinutes = configuration.GetValue<int?>("JwtProperties:ExpiresMinutes") ?? throw new InvalidOperationException("Key not found");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

        var credentials = new SigningCredentials(
            securityKey, 
            SecurityAlgorithms.HmacSha256
        );


        var AccessToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            expires: DateTime.UtcNow.AddMinutes(ExpiresMinutes),

            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(AccessToken);
    }

    public string GenerateRefreshToken()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}











