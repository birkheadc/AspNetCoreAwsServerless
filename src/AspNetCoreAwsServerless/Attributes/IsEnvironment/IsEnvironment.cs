using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAwsServerless.Attributes.IsEnvironment;

public class IsEnvironment(string[] environments) : ActionFilterAttribute
{
  private string[] Environments { get; init; } = environments;

  public override void OnActionExecuting(ActionExecutingContext context)
  {
    string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    if (environment is null || !Environments.Contains(environment))
      context.Result = new ContentResult { StatusCode = 404 };
  }
}
