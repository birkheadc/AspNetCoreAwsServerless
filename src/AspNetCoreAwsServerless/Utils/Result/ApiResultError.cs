using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Utils.Result;

public class ApiResultError
{
  public int StatusCode { get; set; }
  public string ErrorCode { get; set; } = string.Empty;
}
