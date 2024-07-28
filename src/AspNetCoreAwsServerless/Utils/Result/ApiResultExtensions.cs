using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Utils.Result;

public static class ApiResultExtensions
{
  public static IResult ToHttpResult<T>(this ApiResult<T> apiResult)
  {
    if (apiResult.IsSuccess)
    {
      return Results.Ok(apiResult.Value);
    }
    return Results.Problem(
      statusCode: StatusCodes.Status400BadRequest,
      title: "Bad Request",
      extensions: new Dictionary<string, object?> { { "errors", apiResult.Errors } }
    );
  }
}
