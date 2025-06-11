using KonfiguratorSamochodowy.Api.Common.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KonfiguratorSamochodowy.Api.Services;

internal class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateRefreshToken()
    {
        Span<byte> randomNumber = stackalloc byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateToken(int userId, string role, string? name = null)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, $"{userId}"),
            new Claim(ClaimTypes.Role, role),
        ];

        if (!string.IsNullOrEmpty(name))
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        string? secretKey = configuration["JwtInformations:Key"];

        string? issuer = configuration["JwtInformations:Issuer"];

        string? audience = configuration["JwtInformations:Audience"];   

        int expirationTime = int.Parse(configuration["JwtInformations:ExpirationSeconds"] ?? "0");

        if (string.IsNullOrEmpty(secretKey)) throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");
        if (string.IsNullOrEmpty(issuer)) throw new ArgumentNullException(nameof(issuer), "Issuer cannot be null or empty.");
        if (string.IsNullOrEmpty(audience)) throw new ArgumentNullException(nameof(audience), "Audience cannot be null or empty.");
        if (expirationTime <= 0) throw new ArgumentOutOfRangeException(nameof(expirationTime), "Expiration time must be greater than zero.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(expirationTime),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        string? secretKey = configuration["JwtInformations:Key"];

        string? issuer = configuration["JwtInformations:Issuer"];

        string? audience = configuration["JwtInformations:Audience"];

        if (string.IsNullOrEmpty(secretKey)) throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");
        if (string.IsNullOrEmpty(issuer)) throw new ArgumentNullException(nameof(issuer), "Issuer cannot be null or empty.");
        if (string.IsNullOrEmpty(audience)) throw new ArgumentNullException(nameof(audience), "Audience cannot be null or empty.");
       
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            return false;
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
