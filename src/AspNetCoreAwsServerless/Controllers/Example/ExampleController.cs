using AspNetCoreAwsServerless.Dtos.Example;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Example;

/// <summary>
/// Used by the frontend to demo the form framework.
/// </summary>
[ApiController]
[Route("example")]
public class ExampleController(ILogger<ExampleController> logger) : ControllerBase
{
  private readonly ILogger<ExampleController> _logger = logger;

  [HttpPost]
  public Task<ActionResult> PostExample([FromBody] ExampleDto dto)
  {
    _logger.LogInformation("PostExample {dto}", dto);
    return Task.FromResult<ActionResult>(Ok());
  }
}
