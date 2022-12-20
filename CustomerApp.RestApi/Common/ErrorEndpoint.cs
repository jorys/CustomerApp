using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.RestApi.Common;

[ApiExplorerSettings(IgnoreApi = true)]
public sealed class ErrorEndpoint : ControllerBase
{
    [Route("/error")]
    public IActionResult Handle()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return Problem(title: exception?.Message);
    }
}
