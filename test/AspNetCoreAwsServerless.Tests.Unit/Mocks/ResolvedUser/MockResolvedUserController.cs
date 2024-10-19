using AspNetCoreAwsServerless.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using Microsoft.Extensions.Logging;

namespace AspNetCoreAwsServerless.Tests.Unit.Mocks.ResolvedUser;

/// <summary>
/// A dummy implementation of <see cref="ResolvedUserControllerBase{T}"/> for testing purposes.
/// </summary>
public class DummyResolvedUserController(ILogger<DummyResolvedUserController> logger, IUsersService usersService) : ResolvedUserControllerBase<DummyResolvedUserController>(logger, usersService)
{
  public new async Task<User> GetCurrentUser()
  {
    return await base.GetCurrentUser();
  }
}
