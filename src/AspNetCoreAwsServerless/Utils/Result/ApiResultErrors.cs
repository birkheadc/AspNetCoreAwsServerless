using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Utils.Result;

public class ApiResultErrors
{
  public int StatusCode { get; }

  public string? ErrorCode { get; }
  public List<ValidationFailure>? Errors { get; } = [];
  public ObjectResult Problem =>
    new(
      Results.Problem(
        statusCode: StatusCode,
        extensions: new Dictionary<string, object?>
        {
          { "errorCode", ErrorCode },
          { "errors", Errors }
        }
      )
    )
    {
      StatusCode = StatusCode
    };

  public ApiResultErrors(int statusCode)
  {
    StatusCode = statusCode;
  }

  public ApiResultErrors(int statusCode, string errorCode)
  {
    StatusCode = statusCode;
    ErrorCode = errorCode;
  }

  public ApiResultErrors(List<ValidationFailure> errors)
  {
    StatusCode = 400;
    Errors = errors;
  }

  public static readonly ApiResultErrors InternalServerError = new(500);
  public static readonly ApiResultErrors NotFound = new(404);
  public static readonly ApiResultErrors BadRequest = new(400);
  public static readonly ApiResultErrors Unauthorized = new(401);
}
