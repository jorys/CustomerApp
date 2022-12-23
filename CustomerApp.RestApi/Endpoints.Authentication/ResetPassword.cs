using CustomerApp.RestApi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.Application.Handlers.Authentication;

[Route($"{BaseUrl}/reset-password")]
public sealed class ResetPassword : InternalControllerBase
{
    readonly ResetPasswordHandler _commandHandler;

    public ResetPassword(ResetPasswordHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions) 
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost]
    [Tags(SwaggerTags.Authentication)]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Handle(ResetPasswordRequest request)
    {
        var command = request.ToCommand();
        var errorOrAuthenticationResult = await _commandHandler.Handle(command);

        return ToActionResult(
            errorOrAuthenticationResult,
            authenticationResult => NoContent());
    }
}
