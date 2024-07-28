using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Errors;

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
