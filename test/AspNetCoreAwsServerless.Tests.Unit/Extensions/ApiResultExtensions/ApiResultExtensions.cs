using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Tests.Unit.Extensions.ApiResultExtensions;

public static class ApiResultExtensions
{
  public static ApiResultAssertions Should(this ApiResult result)
  {
    return new ApiResultAssertions(result);
  }

  public static ApiResultAssertions<T> Should<T>(this ApiResult<T> result)
  {
    return new ApiResultAssertions<T>(result);
  }
}
