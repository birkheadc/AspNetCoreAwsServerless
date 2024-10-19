using AspNetCoreAwsServerless.Controllers.Example;
using AspNetCoreAwsServerless.Dtos.Example;
using Microsoft.AspNetCore.Mvc;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Example;

public class ExampleControllerTests
{
  private readonly AutoMocker _mocker = new();
  private readonly ExampleController _controller;

  public ExampleControllerTests()
  {
    _controller = _mocker.CreateInstance<ExampleController>();
  }

  [Fact]
  public async Task PostExample_ReturnsOk()
  {
    var dto = new ExampleDto { DisplayName = "Test", Password = "Password123", SecretCode = "1234" };
    var result = await _controller.PostExample(dto);

    Assert.IsType<OkResult>(result);
  }


}
