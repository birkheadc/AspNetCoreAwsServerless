using System.Security.Claims;
using AspNetCoreAwsServerless.Controllers.Session;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Session;
using AspNetCoreAwsServerless.Utils.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Session;

public class SessionControllerTests
{
  private readonly AutoMocker _mocker;
  private readonly SessionController _controller;
  private readonly Mock<ISessionService> _sessionService;
  private readonly Mock<HttpContext> _httpContext;
  private readonly Mock<IAuthenticationService> _authenticationService;
  private readonly Mock<IServiceProvider> _serviceProvider;
  private readonly Mock<IUsersConverter> _usersConverter;
  public SessionControllerTests()
  {
    _mocker = new AutoMocker();
    _controller = _mocker.CreateInstance<SessionController>();
    _sessionService = _mocker.GetMock<ISessionService>();
    _httpContext = _mocker.GetMock<HttpContext>();
    _controller.ControllerContext.HttpContext = _httpContext.Object;
    _authenticationService = _mocker.GetMock<IAuthenticationService>();
    _serviceProvider = _mocker.GetMock<IServiceProvider>();
    _httpContext.Setup(x => x.RequestServices).Returns(_serviceProvider.Object);
    _usersConverter = _mocker.GetMock<IUsersConverter>();
  }

  [Fact]
  public async Task Login_ReturnsFailureResult_WhenLoginFails()
  {
    LoginDto loginDto = _mocker.CreateInstance<LoginDto>();

    _sessionService.Setup(x => x.Login(It.IsAny<LoginDto>())).ReturnsAsync(ApiResult<SessionContext>.BadRequest);

    ActionResult<UserDto> result = await _controller.Login(loginDto);

    result.Should().HaveFailed();
  }

  [Fact]
  public async Task Login_SignsInUser_AndReturnsSuccessResult_WhenLoginSucceeds()
  {
    LoginDto loginDto = _mocker.CreateInstance<LoginDto>();
    User user = _mocker.GetMock<User>().Object;
    user.Id = Guid.NewGuid().ToString();
    user.EmailAddress = "test@test.com";

    SessionContext sessionContext = new()
    {
      User = user,
      Tokens = _mocker.CreateInstance<SessionTokens>()
    };

    UserDto expected = _mocker.CreateInstance<UserDto>();

    _sessionService.Setup(x => x.Login(It.IsAny<LoginDto>())).ReturnsAsync(ApiResult<SessionContext>.Success(sessionContext));

    // HttpContext.SignInAsync cannot be mocked directly, instead mock IAuthenticationService.SignInAsync
    // Then setup the service provider to return the mocked IAuthenticationService
    _authenticationService.Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);
    _serviceProvider.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(_authenticationService.Object);

    _httpContext.Setup(x => x.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()));

    _usersConverter.Setup(x => x.ToDto(It.IsAny<User>())).Returns(expected);

    ActionResult<UserDto> result = await _controller.Login(loginDto);

    // As HttpContext.SignInAsync cannot be mocked directly, we need to verify that the mocked IAuthenticationService.SignInAsync was called
    _authenticationService.Verify(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Once);
    _httpContext.Verify(x => x.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);

    result.Should().HaveValue(expected);
  }
}
