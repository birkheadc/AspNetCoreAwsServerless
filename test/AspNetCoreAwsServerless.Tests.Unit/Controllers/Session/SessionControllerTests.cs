using AspNetCoreAwsServerless.Controllers.Session;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Services.Session;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Session;

public class SessionControllerTests
{
  private readonly AutoMocker _mocker;
  private readonly SessionController _controller;
  private readonly Mock<ISessionService> _sessionService;

  public SessionControllerTests()
  {
    _mocker = new AutoMocker();
    _controller = _mocker.CreateInstance<SessionController>();
    _sessionService = _mocker.GetMock<ISessionService>();
  }

  [Fact]
  public async Task Login_ReturnsFailureResult_WhenLoginFails()
  {
    _sessionService.Setup(x => x.Login(It.IsAny<LoginDto>())).ReturnsAsync(ApiResult<SessionContext>.BadRequest);

    ActionResult<UserDto> result = await _controller.Login(_mocker.GetMock<LoginDto>().Object);

    Assert.IsType<BadRequestObjectResult>(result.Result);
  }
}
