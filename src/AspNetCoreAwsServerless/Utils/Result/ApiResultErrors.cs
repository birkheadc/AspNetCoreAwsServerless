using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Utils.Result;

public class ApiResultErrors
{
  public int StatusCode { get; }
  public List<ApiResultError> Errors { get; } = [];
  public ObjectResult Problem =>
    (ObjectResult)
      Results.Problem(
        statusCode: StatusCode,
        title: "Bad Request",
        extensions: new Dictionary<string, object?> { { "errors", Errors } }
      );

  public ApiResultErrors(int statusCode)
  {
    StatusCode = statusCode;
  }

  public ApiResultErrors(int statusCode, ApiResultError error)
  {
    StatusCode = statusCode;
    Errors.Add(error);
  }

  public ApiResultErrors(int statusCode, IEnumerable<ApiResultError> errors)
  {
    StatusCode = statusCode;
    Errors = errors.ToList();
  }

  public ApiResultErrors(int statusCode, List<ApiResultError> errors)
  {
    StatusCode = statusCode;
    Errors = errors;
  }

  public static readonly ApiResultErrors InternalServerError = new(500);
}
