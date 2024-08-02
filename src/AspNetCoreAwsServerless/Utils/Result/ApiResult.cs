using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
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

  public ObjectResult GetObjectResult()
  {
    return IsSuccess ? (ObjectResult)Results.Ok() : Errors.Problem;
  }

  public static ApiResult Success() => new();

  public static ApiResult<T> Success<T>(T value) => new(value);

  public static ApiResult Failure(ApiResultErrors errors) => new(errors);

  public static implicit operator ApiResult(ApiResultErrors errors) => new(errors);
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

  public ObjectResult GetObjectResult()
  {
    return IsSuccess ? new ObjectResult(Value) { StatusCode = 200 } : Errors.Problem;
  }

  public ObjectResult GetObjectResult(Func<T, object> converter)
  {
    return IsSuccess ? new ObjectResult(converter(Value)) { StatusCode = 200 } : Errors.Problem;
  }

  public static ApiResult<T> Success(T value) => new(value);

  public static ApiResult<T> Failure(ApiResultErrors errors) => new(errors);

  public static implicit operator ApiResult<T>(T value) => new(value);

  public static implicit operator ApiResult<T>(ApiResultErrors errors) => new(errors);
}
