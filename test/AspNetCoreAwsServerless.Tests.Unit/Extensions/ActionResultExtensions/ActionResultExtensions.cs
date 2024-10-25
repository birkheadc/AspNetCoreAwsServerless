using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Tests.Unit.Extensions.ActionResultExtensions;

public static class ActionResultExtensions
{
  public static ActionResultAssertions Should(this ActionResult result)
  {
    return new ActionResultAssertions(result);
  }

  public static ActionResultAssertions<T> Should<T>(this ActionResult<T> result)
  {
    return new ActionResultAssertions<T>(result);
  }
}
