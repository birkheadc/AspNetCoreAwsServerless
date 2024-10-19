using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Errors;

/// <summary>
/// Currently not implemented.
/// </summary>
[ApiController]
[Route("errors")]
public class ErrorsController : ControllerBase
{
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult HandleError()
  {
    return NotFound();
  }
}
