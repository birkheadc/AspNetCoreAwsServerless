using AspNetCoreAwsServerless.Dtos.Sums;
using AspNetCoreAwsServerless.Services.Sums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Sums;

/// <summary>
/// An simple example controller that provides a single endpoint for summing two numbers. Can probably be removed soon.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("sums")]
public class SumsController(ISumsService sumsService, ILogger<SumsController> logger) : ControllerBase
{
  private readonly ISumsService _sumsService = sumsService;
  private readonly ILogger<SumsController> _logger = logger;

  [HttpPost]
  public async Task<ActionResult<int>> Sum([FromBody] SumCreateDto dto)
  {
    _logger.LogInformation("SumsController.Sum");
    return Ok(await _sumsService.Sum(dto));
  }
}
