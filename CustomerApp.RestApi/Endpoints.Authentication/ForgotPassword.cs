using CustomerApp.Application.Handlers.Authentication;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.Customers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Authentication;

[Route("api/forgot-password")]
public sealed class ForgotPassword : InternalControllerBase
{
    readonly ForgotPasswordHandler _commandHandler;

    public ForgotPassword(ForgotPasswordHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost]
    [Tags("Authentication")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Handle(ForgotPasswordRequest request)
    {
        var command = request.ToCommand();
        var errorOrAuthenticationResult = await _commandHandler.Handle(command);
        return ToActionResult(
            errorOrAuthenticationResult,
            authenticationResult => NoContent());
    }
}
