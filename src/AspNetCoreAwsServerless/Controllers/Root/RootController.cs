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
    return await Task.Run(() => Ok(_config.Greeting));
  }

  [HttpGet]
  [Route("/e")]
  public IActionResult ThrowError()
  {
    throw new Exception("Intentional Exception");
  }

  [HttpGet]
  [Route("/secure")]
  [Authorize]
  public async Task<ActionResult<string>> GetSecure()
  {
    return await Task.Run(() => Ok(_config.Greeting));
  }
}
