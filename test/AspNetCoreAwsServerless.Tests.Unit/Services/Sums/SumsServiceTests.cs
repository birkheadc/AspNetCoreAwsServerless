using AspNetCoreAwsServerless.Dtos.Sums;
using AspNetCoreAwsServerless.Services.Sums;
using Moq.AutoMock;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Services.Sums;

public class SumsServiceTests
{
  private readonly AutoMocker _mocker;
  private readonly SumsService _service;

  public SumsServiceTests()
  {
    _mocker = new();
    _service = _mocker.CreateInstance<SumsService>();
  }

  [Theory, MemberData(nameof(Sum_ReturnsSumOfAllValues_Data))]
  public async void Sum_ReturnsSumOfAllValues(SumCreateDto dto, int expected)
  {
    int actual = await _service.Sum(dto);

    Assert.Equal(expected, actual);
  }

  public static IEnumerable<object[]> Sum_ReturnsSumOfAllValues_Data()
  {
    return
    [
      [new SumCreateDto() { Values = [0, 1, 2, 0] }, 3],
      [new SumCreateDto() { Values = [100, 60, 1000, 1] }, 1161],
      [new SumCreateDto() { Values = [0, 0, 0, 0, 0, 0, 0] }, 0]
    ];
  }
}
