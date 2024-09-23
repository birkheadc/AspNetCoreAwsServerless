using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Attributes.ResolveUser;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAwsServerless.Filters.ResolveUserFilter;

public class ResolveUserFilter(IUsersService usersService) : IAsyncActionFilter
{
  private readonly IUsersService _usersService = usersService;

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    Console.WriteLine("ResolveUserFilter");
    var parameters = context.ActionDescriptor.Parameters
        .Where(p => p.ParameterType == typeof(User) && p.BindingInfo?.BindingSource?.Id == "ResolveUser")
        .ToList();

    Console.WriteLine("parameters: " + parameters.Count);
    if (parameters.Count > 0)
    {
      string? id = (context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value) ?? throw new UnauthorizedAccessException();
      Console.WriteLine("id: " + id);
      ApiResult<User> result = await _usersService.Get(new Id<User>(id));

      if (result.IsFailure)
      {
        Console.WriteLine("result.IsFailure: " + result.IsFailure);
        context.Result = result.GetActionResult();
        return;
      }

      foreach (var parameter in parameters)
      {
        Console.WriteLine("parameter.Name: " + parameter.Name);
        context.ActionArguments[parameter.Name] = result.Value;
      }
    }

    await next();
  }
}
