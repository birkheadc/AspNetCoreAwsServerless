using System.Security.Claims;
using AspNetCoreAwsServerless.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Tests.Unit.Mocks.ResolvedUser;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.ResolvedUser;

public class ResolvedUserControllerBaseTests
{
  protected readonly AutoMocker _mocker = new();
  private readonly DummyResolvedUserController _controller;
  protected readonly Mock<HttpContext> _httpContext;
  protected readonly Mock<IUsersService> _usersService;
  public ResolvedUserControllerBaseTests()
  {
    _controller = _mocker.CreateInstance<DummyResolvedUserController>();
    _httpContext = new Mock<HttpContext>();
    _controller.ControllerContext.HttpContext = _httpContext.Object;
    _usersService = _mocker.GetMock<IUsersService>();
  }

  protected void SetupGetCurrentUser(User user)
  {
    _httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())])));
    _usersService.Setup(x => x.Get(user.Id)).ReturnsAsync(user);
  }

  [Fact]
  public async Task GetCurrentUser_ThrowsUnauthorizedAccessException_ClaimNotFound()
  {
    _httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));
    await Assert.ThrowsAsync<UnauthorizedAccessException>(_controller.GetCurrentUser);
  }

  [Fact]
  public async Task GetCurrentUser_ThrowsUnauthorizedAccessException_UserNotFound()
  {
    _httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())])));
    _usersService.Setup(x => x.Get(It.IsAny<Id<User>>())).ReturnsAsync(ApiResult<User>.NotFound);
    await Assert.ThrowsAsync<UnauthorizedAccessException>(_controller.GetCurrentUser);
  }

  [Fact]
  public async Task GetCurrentUser_ThrowsUnauthorizedAccessException_InvalidUserId()
  {
    _httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "123")])));
    await Assert.ThrowsAsync<UnauthorizedAccessException>(_controller.GetCurrentUser);
  }

  [Fact]
  public async Task GetCurrentUser_ReturnsUser()
  {
    User expected = new()
    {
      Id = new Id<User>(Guid.NewGuid()),
      EmailAddress = "test@test.com",
    };

    SetupGetCurrentUser(expected);

    User actual = await _controller.GetCurrentUser();
    Assert.Equal(expected, actual);
  }
}
