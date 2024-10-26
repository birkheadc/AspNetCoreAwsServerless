using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Errors;

/// <summary>
/// Can be used to handle errors. Currently not implemented.
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
