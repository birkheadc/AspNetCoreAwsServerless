using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Utils.Result;

public class ApiResult
{
  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;
  public ApiResultError? Error { get; }

  private ApiResult()
  {
    IsSuccess = true;
    Error = default;
  }

  public ApiResult(ApiResultError error)
  {
    IsSuccess = false;
    Error = error;
  }

  public static ApiResult Success() => new() { };

  public static ApiResult<T> Success<T>(T value) => new(value);

  public static ApiResult Failure(ApiResultError error) => new(error);
}

public class ApiResult<T>
{
  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;
  public T? Value { get; }
  public ApiResultError? Error { get; }

  public ApiResult(T value)
  {
    IsSuccess = true;
    Value = value;
    Error = default;
  }

  public ApiResult(ApiResultError error)
  {
    IsSuccess = false;
    Value = default;
    Error = error;
  }

  public static ApiResult<T> Success(T value) => new(value);

  public static ApiResult<T> Failure(ApiResultError error) => new(error);

  public static implicit operator ApiResult<T>(ApiResultError error) => ApiResult<T>.Failure(error);
}
