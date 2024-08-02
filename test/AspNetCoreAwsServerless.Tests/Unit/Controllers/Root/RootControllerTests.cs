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

  [Fact]
  public async Task Get_ReturnsGreeting()
  {
    string expected = options.Greeting;

    AutoMocker mocker = new();

    mocker.GetMock<IOptions<RootOptions>>().SetupGet(mock => mock.Value).Returns(options);

    RootController controller = mocker.CreateInstance<RootController>();
    ActionResult<string> result = await controller.Get();

    OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal(expected, okObjectResult.Value);
  }

  [Fact]
  public async Task GetSecure_ReturnsGreeting()
  {
    string expected = options.Greeting;

    AutoMocker mocker = new();

    mocker.GetMock<IOptions<RootOptions>>().SetupGet(mock => mock.Value).Returns(options);

    RootController controller = mocker.CreateInstance<RootController>();
    ActionResult<string> result = await controller.GetSecure();

    OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal(expected, okObjectResult.Value);
  }
}
