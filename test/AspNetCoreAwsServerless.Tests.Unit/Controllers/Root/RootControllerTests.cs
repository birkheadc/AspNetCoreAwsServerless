using AspNetCoreAwsServerless.Config.Root;
using AspNetCoreAwsServerless.Controllers.Root;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Root;

public class RootControllerTests
{
  private readonly RootOptions options = new() { Greeting = "Test Greeting!" };

  private readonly AutoMocker _mocker;
  private readonly RootController _controller;

  public RootControllerTests()
  {
    _mocker = new();
    _mocker.GetMock<IOptions<RootOptions>>().SetupGet(mock => mock.Value).Returns(options);
    _controller = _mocker.CreateInstance<RootController>();
  }

  [Fact]
  public async Task Get_ReturnsGreeting()
  {
    string expected = options.Greeting;

    ActionResult<string> result = await _controller.Get();
    result.Should().HaveValue(expected);
  }

  [Fact]
  public void ThrowError_ThrowsError()
  {
    Func<IActionResult> action = _controller.ThrowError;
    action.Should().Throw<Exception>();
  }

  [Fact]
  public async Task GetSecure_ReturnsGreeting()
  {
    string expected = options.Greeting;

    ActionResult<string> result = await _controller.GetSecure();

    result.Should().HaveValue(expected);
  }
}
