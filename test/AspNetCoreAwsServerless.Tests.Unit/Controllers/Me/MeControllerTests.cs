using AspNetCoreAwsServerless.Controllers.Me;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Tests.Unit.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Utils.Id;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Me;

public class MeControllerTests : ResolvedUserControllerBaseTests
{
  private readonly MeController _controller;
  private readonly Mock<IUsersConverter> _usersConverter;

  public MeControllerTests() : base()
  {
    _controller = _mocker.CreateInstance<MeController>();
    _usersConverter = _mocker.GetMock<IUsersConverter>();
    _controller.ControllerContext.HttpContext = _httpContext.Object;
  }

  [Fact]
  public async Task Get_ReturnsCurrentUserDto()
  {
    UserDto expected = _mocker.GetMock<UserDto>().Object;

    User user = _mocker.GetMock<User>().Object;
    user.Id = Guid.NewGuid().ToString();

    _usersConverter.Setup(x => x.ToDto(user)).Returns(expected);

    SetupGetCurrentUser(user);

    ActionResult<UserDto> result = await _controller.Get();
    result.Should().HaveValue(expected);
  }
}
