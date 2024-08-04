using Amazon.DynamoDBv2.Model;
using AspNetCoreAwsServerless.Config.Root;
using AspNetCoreAwsServerless.Controllers.Root;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq.AutoMock;
using Xunit;

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

    OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal(expected, okObjectResult.Value);
  }

  [Fact]
  public void ThrowError_ThrowsError()
  {
    Assert.Throws<Exception>(_controller.ThrowError);
  }

  [Fact]
  public async Task GetSecure_ReturnsGreeting()
  {
    string expected = options.Greeting;

    ActionResult<string> result = await _controller.GetSecure();

    OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal(expected, okObjectResult.Value);
  }
}
