using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetCoreAwsServerless.Services.Jwt;

public class JwtService : IJwtService
{
  public IEnumerable<Claim> Decode(string token)
  {
    JwtSecurityTokenHandler handler = new();
    JwtSecurityToken jsonToken = handler.ReadJwtToken(token);

    return jsonToken.Claims;
  }
}
