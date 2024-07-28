using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Utils.Result;

public class ApiResult
{
  public bool IsSuccess { get; set; }
  public bool IsFailure => !IsSuccess;
  public ApiResultError? Error { get; set; }

  public static ApiResult Success() => new() { IsSuccess = true };

  public static ApiResult<T> Success<T>(T value) => new() { IsSuccess = true, Value = value };

  public static ApiResult Failure(ApiResultError error) =>
    new() { IsSuccess = false, Error = error };
}

public class ApiResult<T>
{
  public bool IsSuccess { get; set; }
  public bool IsFailure => !IsSuccess;
  public T? Value { get; set; }
  public ApiResultError? Error { get; set; }

  public static ApiResult<T> Success(T value) => new() { IsSuccess = true, Value = value };

  public static ApiResult<T> Failure(ApiResultError error) =>
    new() { IsSuccess = false, Error = error };

  public static implicit operator ApiResult<T>(ApiResultError error) => ApiResult<T>.Failure(error);
}
