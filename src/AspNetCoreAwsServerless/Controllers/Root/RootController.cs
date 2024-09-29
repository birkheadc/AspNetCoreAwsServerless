using AspNetCoreAwsServerless.Config.Root;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCoreAwsServerless.Controllers.Root;

[ApiController]
[Route("")]
public class RootController(IOptions<RootOptions> config) : ControllerBase
{
  private readonly RootOptions _config = config.Value;

  [HttpGet]
  public async Task<ActionResult<string>> Get()
  {
    string clientId =
      Environment.GetEnvironmentVariable("ASPNETCORE_COGNITO_CLIENT_ID")
      ?? throw new Exception("ASPNETCORE_COGNITO_CLIENT_ID not set");
    string userPoolId =
      Environment.GetEnvironmentVariable("ASPNETCORE_COGNITO_USER_POOL_ID")
      ?? throw new Exception("ASPNETCORE_COGNITO_USER_POOL_ID not set");
    return await Task.Run(() => Ok($"user pool id = {userPoolId} | client id = {clientId}"));
    return await Task.Run(() => Ok(_config.Greeting));
  }

  [HttpGet]
  [Route("e")]
  public IActionResult ThrowError()
  {
    throw new Exception();
  }

  [HttpGet]
  [Route("secure")]
  // [Authorize]
  public async Task<ActionResult<string>> GetSecure()
  {
    return await Task.Run(() => Ok(_config.Greeting));
  }
}
