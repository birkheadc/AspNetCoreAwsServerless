using AspNetCoreAwsServerless.Dtos.Sums;

namespace AspNetCoreAwsServerless.Services.Sums;

public interface ISumsService
{
  public Task<int> Sum(SumCreateDto dto);
}
