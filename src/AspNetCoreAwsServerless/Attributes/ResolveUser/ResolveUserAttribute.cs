using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Caches.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAwsServerless.Attributes.ResolveUser;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ResolveUserAttribute : Attribute, IAsyncAuthorizationFilter
{
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    ILogger<ResolveUserAttribute> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ResolveUserAttribute>>();
    string? accessToken = context.HttpContext.Request.Cookies["access_token"];

    if (accessToken is null)
    {
      logger.LogWarning("No access token found in cookies");
      context.Result = new UnauthorizedResult();
      return;
    }

    ISessionCache sessionCache = context.HttpContext.RequestServices.GetRequiredService<ISessionCache>();
    ApiResult<Id<User>> userIdResult = await sessionCache.GetUserId(accessToken);
    if (userIdResult.IsFailure)
    {
      logger.LogWarning("No user ID found for access token");
      context.Result = new UnauthorizedResult();
      return;
    }

    logger.LogInformation("Signing in user {UserId}", userIdResult.Value);
    ClaimsPrincipal claimsPrincipal = new();
    claimsPrincipal.AddIdentity(new ClaimsIdentity([new Claim("sub", userIdResult.Value.ToString())], CookieAuthenticationDefaults.AuthenticationScheme));

    await context.HttpContext.SignInAsync(claimsPrincipal);


    // string? id = context.HttpContext.User.Claims.Where(c => c.Type == "username").FirstOrDefault()?.Value;
    // string? accessToken = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?["Bearer ".Length..];

    // ApiResult<User> userResult = await usersService.GetOrCreateNew(id, accessToken);

    // if (userResult.IsFailure)
    // {
    //   context.Result = new UnauthorizedResult();
    //   return;
    // }

    // Console.WriteLine($"Add this user to context or whatever: {userResult.Value.Id}");
    // // Add this user to context or whatever
    // context.HttpContext.Items["user"] = userResult.Value;
    // await next();

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