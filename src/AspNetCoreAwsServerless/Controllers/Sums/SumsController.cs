using AspNetCoreAwsServerless.Dtos.Sums;
using AspNetCoreAwsServerless.Services.Sums;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Sums;

[ApiController]
[Route("sums")]
public class SumsController(ISumsService sumsService) : ControllerBase
{
  private readonly ISumsService _sumsService = sumsService;

  [HttpPost]
  public async Task<int> Sum([FromBody] SumCreateDto dto)
  {
    return await _sumsService.Sum(dto);
  }
}
