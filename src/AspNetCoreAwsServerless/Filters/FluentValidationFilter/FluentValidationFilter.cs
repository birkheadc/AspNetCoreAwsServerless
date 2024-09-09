using AspNetCoreAwsServerless.Utils.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AspNetCoreAwsServerless.Filters.FluentValidationFilter;

/// <summary>
/// Attempts to validate arguments to controller methods automatically. Rejects invalid requests with the proper error details.
/// </summary>
public class FluentValidationFilter : IAsyncActionFilter
{
  public void OnActionExecuted(ActionExecutedContext context)
  {
    // throw new NotImplementedException();
  }

  public void OnActionExecuting(ActionExecutingContext context) { }

  public async Task OnActionExecutionAsync(
    ActionExecutingContext context,
    ActionExecutionDelegate next
  )
  {
    IServiceProvider services = context.HttpContext.RequestServices;
    ICollection<object?> arguments = context.ActionArguments.Values;

    foreach (var argument in arguments)
    {
      if (argument is null)
        continue;

      IValidator? validator = GetValidatorForArgument(argument, services);

      if (validator is null)
        continue;

      ValidationContext<object> validationContext = new(argument);
      ValidationResult validationResult = await validator.ValidateAsync(validationContext);

      if (!validationResult.IsValid)
      {
        ActionResult result = ApiResult.Failure(validationResult).GetActionResult();

        ILogger<FluentValidationFilter>? logger = services.GetService<
          ILogger<FluentValidationFilter>
        >();
        if (logger is not null)
        {
          logger.LogWarning(
            "Rejected invalid request: {result}",
            JsonConvert.SerializeObject(result)
          );
        }

        context.Result = result;
        return;
      }
    }
    await next();
  }

  private static IValidator? GetValidatorForArgument(object argument, IServiceProvider services)
  {
    Type type = argument.GetType();

    if (services.GetService(typeof(IValidator<>).MakeGenericType(type)) is IValidator validator)
      return validator;

    return null;
  }
}
