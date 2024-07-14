using AspNetCoreAwsServerless.Dtos.Sums;

namespace AspNetCoreAwsServerless.Services.Sums;

public class SumsService : ISumsService
{
  public async Task<int> Sum(SumCreateDto dto)
  {
    int sum = 0;

    foreach (int n in dto.Values)
    {
      sum += n;
    }

    return await Task.Run(() => sum);
  }
}
