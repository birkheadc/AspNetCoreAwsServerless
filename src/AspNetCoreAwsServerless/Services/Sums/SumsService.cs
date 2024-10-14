using AspNetCoreAwsServerless.Dtos.Sums;

namespace AspNetCoreAwsServerless.Services.Sums;

public class SumsService(ILogger<SumsService> logger) : ISumsService
{
  private readonly ILogger<SumsService> _logger = logger;

  public async Task<int> Sum(SumCreateDto dto)
  {
    _logger.LogInformation("Attempting to sum {Values}", dto.Values);
    int sum = 0;

    foreach (int n in dto.Values)
    {
      sum += n;
    }

    return await Task.Run(() => sum);
  }
}
