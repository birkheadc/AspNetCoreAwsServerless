using AspNetCoreAwsServerless.Dtos.Example;
using AspNetCoreAwsServerless.Utils.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Example;

[ApiController]
[Route("example")]
public class ExampleController(ILogger<ExampleController> logger) : ControllerBase
{
  private readonly ILogger<ExampleController> _logger = logger;

  [HttpPost]
  public async Task<ActionResult> PostExample([FromBody] ExampleDto dto)
  {
    _logger.LogInformation("PostExample");
    return Ok();
  }
}
