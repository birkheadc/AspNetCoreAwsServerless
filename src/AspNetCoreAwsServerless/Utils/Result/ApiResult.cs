using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Utils.Result;

public class ApiResult
{
  [MemberNotNullWhen(returnValue: false, nameof(Errors))]
  public bool IsSuccess { get; }

  [MemberNotNullWhen(returnValue: true, nameof(Errors))]
  public bool IsFailure => !IsSuccess;
  public ApiResultErrors? Errors { get; }

  public ApiResult()
  {
    IsSuccess = true;
    Errors = default;
  }

  public ApiResult(ApiResultErrors errors)
  {
    IsSuccess = false;
    Errors = errors;
  }

  public ApiResult(ValidationResult validationResult)
  {
    IsSuccess = validationResult.IsValid;
    Errors = new(validationResult.Errors);
  }

  public ActionResult GetActionResult()
  {
    return IsSuccess ? new OkResult() : Errors.Problem;
  }

  public static ApiResult Success() => new();

  public static ApiResult<T> Success<T>(T value) => new(value);

  public static ApiResult Failure(ApiResultErrors errors) => new(errors);

  public static implicit operator ApiResult(ApiResultErrors errors) => new(errors);

  public static ApiResult Failure(ValidationResult validationResult) => new(validationResult);
  public static ApiResult NotFound => new(ApiResultErrors.NotFound);
  public static ApiResult BadRequest => new(ApiResultErrors.BadRequest);
  public static ApiResult Unauthorized => new(ApiResultErrors.Unauthorized);
  public static ApiResult InternalServerError => new(ApiResultErrors.InternalServerError);
}

public class ApiResult<T>
{
  [MemberNotNullWhen(returnValue: true, nameof(Value))]
  [MemberNotNullWhen(returnValue: false, nameof(Errors))]
  public bool IsSuccess { get; }

  [MemberNotNullWhen(returnValue: false, nameof(Value))]
  [MemberNotNullWhen(returnValue: true, nameof(Errors))]
  public bool IsFailure => !IsSuccess;
  public T? Value { get; }
  public ApiResultErrors? Errors { get; }

  public ApiResult(T value)
  {
    IsSuccess = true;
    Value = value;
    Errors = default;
  }

  public ApiResult(ApiResultErrors errors)
  {
    IsSuccess = false;
    Value = default;
    Errors = errors;
  }

  public ApiResult(ValidationResult validationResult)
  {
    IsSuccess = validationResult.IsValid;
    Value = default;
    Errors = new(validationResult.Errors);
  }

  public ActionResult GetActionResult()
  {
    return IsSuccess ? new OkResult() : Errors.Problem;
  }

  public ActionResult GetActionResult(Func<T, object> converter)
  {
    return IsSuccess ? new OkObjectResult(converter(Value)) : Errors.Problem;
  }

  public static ApiResult<T> Success(T value) => new(value);

  public static ApiResult<T> Failure(ApiResultErrors errors) => new(errors);

  public static ApiResult Failure(ValidationResult validationResult) => new(validationResult);

  public static implicit operator ApiResult<T>(T value) => new(value);

  public static implicit operator ApiResult<T>(ApiResultErrors errors) => new(errors);

  public static ApiResult<T> NotFound => new(ApiResultErrors.NotFound);
  public static ApiResult<T> BadRequest => new(ApiResultErrors.BadRequest);
  public static ApiResult<T> Unauthorized => new(ApiResultErrors.Unauthorized);
  public static ApiResult<T> InternalServerError => new(ApiResultErrors.InternalServerError);
}
