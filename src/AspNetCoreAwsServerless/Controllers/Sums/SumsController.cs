using AspNetCoreAwsServerless.Dtos.Sums;
using AspNetCoreAwsServerless.Services.Sums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Sums;

[ApiController]
[AllowAnonymous]
[Route("sums")]
public class SumsController(ISumsService sumsService) : ControllerBase
{
  private readonly ISumsService _sumsService = sumsService;

  [HttpPost]
  public async Task<ActionResult<int>> Sum([FromBody] SumCreateDto dto)
  {
    return Ok(await _sumsService.Sum(dto));
  }
}
