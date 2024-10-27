using System.Security.Claims;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Tests.Unit.Mocks.ResolvedUser;
using AspNetCoreAwsServerless.Utils.Id;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.ResolvedUser;

public class ResolvedUserControllerBaseTests
{
  protected readonly AutoMocker _mocker = new();
  private readonly DummyResolvedUserController _controller;
  protected readonly Mock<HttpContext> _httpContext;

  public ResolvedUserControllerBaseTests()
  {
    _controller = _mocker.CreateInstance<DummyResolvedUserController>();
    _httpContext = new Mock<HttpContext>();
    _controller.ControllerContext.HttpContext = _httpContext.Object;
  }

  protected void SetupGetCurrentUserId(Id<User> id)
  {
    _httpContext
      .Setup(x => x.User)
      .Returns(
        new ClaimsPrincipal(
          new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, id.ToString())])
        )
      );
  }

  // TODO: Change all these tests for GetCurrentUserId

  [Fact]
  public void GetCurrentUserId_ThrowsUnauthorizedAccessException_ClaimNotFound()
  {
    _httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));
    Action action = () => _controller.GetCurrentUserId();
    action.Should().Throw<UnauthorizedAccessException>();
  }

  [Fact]
  public void GetCurrentUserId_ThrowsUnauthorizedAccessException_InvalidUserId()
  {
    _httpContext
      .Setup(x => x.User)
      .Returns(
        new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "123")]))
      );
    Action action = () => _controller.GetCurrentUserId();
    action.Should().Throw<UnauthorizedAccessException>();
  }
}
