using AspNetCoreAwsServerless.Controllers.Root;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Root
{
  public class RootControllerTests
  {
    [Fact]
    public async void Get_ReturnsGreeting()
    {
      RootController controller = new();

      string GREETING =
        $"You have reached Colby's ASP.NET Core Aws Serverless Template API Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
      string expected = GREETING;

      string actual = await controller.Get();

      Assert.Equal(expected, actual);
    }
  }
}
