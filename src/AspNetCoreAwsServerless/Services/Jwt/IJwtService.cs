using System.Security.Claims;

namespace AspNetCoreAwsServerless.Services.Jwt;

public interface IJwtService
{
  public IEnumerable<Claim> Decode(string token);
}
