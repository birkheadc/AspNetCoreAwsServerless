using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAwsServerless.Attributes.ResolveUser;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ResolveUserAttribute : Attribute, IAsyncResourceFilter
{
  public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
  {
    IUsersService usersService = context.HttpContext.RequestServices.GetRequiredService<IUsersService>();

    string? id = context.HttpContext.User.Claims.Where(c => c.Type == "username").FirstOrDefault()?.Value;
    string? accessToken = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?["Bearer ".Length..];

    ApiResult<User> userResult = await usersService.GetOrCreateNew(id, accessToken);

    if (userResult.IsFailure)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    Console.WriteLine($"Add this user to context or whatever: {userResult.Value.Id}");
    // Add this user to context or whatever
    context.HttpContext.Items["user"] = userResult.Value;
    await next();
    // _logger.LogDebug("ResolveUserAttribute OnResourceExecutionAsync");
    // IUsersService usersService = context.HttpContext.RequestServices.GetRequiredService<IUsersService>();
    // string? id = context.HttpContext.User.Claims.Where(c => c.Type == "username").FirstOrDefault()?.Value;
    // if (id is null)
    // {
    //   _logger.LogWarning("Unauthorized Login (No 'username' claim provided).");
    //   context.Result = new UnauthorizedResult();
    //   return;
    // }

    // ApiResult<User> userResult = await usersService.Get(new Id<User>(id));
    // if (userResult.IsFailure)
    // {
    //   _logger.LogDebug("User not found, creating...");
    //   string? accessToken = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?["Bearer ".Length..];
    //   if (accessToken is null)
    //   {
    //     _logger.LogError("Error extracting access token from authorization header.");
    //     context.Result = new UnauthorizedResult();
    //     return;
    //   }
    //   ApiResult<User> createUserResult = await usersService.CreateNew(accessToken);
    //   if (createUserResult.IsFailure)
    //   {
    //     _logger.LogError("Failed to create new user, Id: {id}", id);
    //     context.Result = createUserResult.GetActionResult();
    //     return;
    //   }
    //   _logger.LogDebug("Created User: {id}", createUserResult.Value.Id);
    // }
    // else
    // {
    //   _logger.LogDebug("User found: {id}", userResult.Value.Id);
    // }

    // ParameterDescriptor? parameter = context.ActionDescriptor.Parameters
    //   .FirstOrDefault(p => p.ParameterType == typeof(User) &&
    //   p.ParameterType.CustomAttributes.Any(a => a.AttributeType == typeof(ResolveUserAttribute)));

    // if (parameter != null)
    // {
    //   Console.WriteLine("Parameter found");
    //   context.HttpContext.Items[parameter.Name] = user.Value;
    // }
  }
}