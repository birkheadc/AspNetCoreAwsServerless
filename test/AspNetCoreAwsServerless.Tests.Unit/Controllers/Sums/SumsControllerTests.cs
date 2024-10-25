using AspNetCoreAwsServerless.Controllers.Sums;
using AspNetCoreAwsServerless.Dtos.Sums;
using AspNetCoreAwsServerless.Services.Sums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Sums;

public class SumsControllerTests
{
  private readonly SumsController _controller;
  private readonly AutoMocker _mocker;
  private readonly Mock<ISumsService> _sumsService;

  public SumsControllerTests()
  {
    _mocker = new AutoMocker();
    _controller = _mocker.CreateInstance<SumsController>();
    _sumsService = _mocker.GetMock<ISumsService>();
  }

  [Fact]
  public async Task Sum_ReturnsResultOfSumService()
  {
    _sumsService.Setup(x => x.Sum(It.IsAny<SumCreateDto>())).ReturnsAsync(1);

    ActionResult<int> result = await _controller.Sum(_mocker.GetMock<SumCreateDto>().Object);

    Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal(1, ((OkObjectResult)result.Result).Value);
  }
}
