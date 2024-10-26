using AspNetCoreAwsServerless.Config.Root;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCoreAwsServerless.Controllers.Root;

/// <summary>
/// Provides the root endpoint greeting. Error and secure endpoints should be moved to a debugging controller.
/// </summary>
[ApiController]
[Route("")]
public class RootController(IOptions<RootOptions> config, ILogger<RootController> logger) : ControllerBase
{
  private readonly RootOptions _config = config.Value;
  private readonly ILogger<RootController> _logger = logger;

  [HttpGet]
  public async Task<ActionResult<string>> Get()
  {
    _logger.LogInformation("RootController.Get");
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
  [Authorize]
  public async Task<ActionResult<string>> GetSecure()
  {
    return await Task.Run(() => Ok(_config.Greeting));
  }
}
