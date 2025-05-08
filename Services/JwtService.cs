using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace e_commerce_cs.Services
{
  public class JwtService(IConfiguration config)
  {
    private readonly string _secretKey = config["JwtSettings:SecretKey"]
        ?? throw new ArgumentNullException(nameof(config), "Sem secret key definida.");
    private readonly string _audience = config["JwtSettings:Audience"]
        ?? throw new ArgumentNullException(nameof(config), "Sem audience definido.");
    private readonly int _expirationMinutes = 60;

    public string GenerateToken(string userId, string userEmail)
    {
      SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secretKey));
      SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

      Claim[] claims =
      [
          new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      ];

      JwtSecurityToken token = new(
          audience: _audience,
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
          signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
      try
      {
        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters validationParameters = new()
        {
          ValidateIssuer = false,
          ValidateAudience = true,
          ValidAudience = _audience,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
        };

        return tokenHandler.ValidateToken(token, validationParameters, out _);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
