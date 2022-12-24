using CustomerApp.Application.Handlers.Authentication;
using CustomerApp.RestApi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Authentication;

[Route($"{BaseUrl}/forgot-password")]
public sealed class ForgotPassword : InternalControllerBase
{
    readonly ForgotPasswordHandler _commandHandler;

    public ForgotPassword(ForgotPasswordHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost]
    [Tags(SwaggerTags.Authentication)]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Handle(ForgotPasswordRequest request, CancellationToken ct)
    {
        var command = request.ToCommand();
        var errorOrAuthenticationResult = await _commandHandler.Handle(command, ct);

        return ToActionResult(
            errorOrAuthenticationResult,
            authenticationResult => NoContent());
    }
}
