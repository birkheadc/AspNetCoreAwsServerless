using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Root;

[ApiController]
[Route("")]
public class RootController : ControllerBase
{
  [HttpGet]
  public async Task<string> Get()
  {
    return await Task.Run(
      () =>
        $"You have reached Colby's ASP.NET Core Aws Serverless Template API Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}"
    );
  }
}
