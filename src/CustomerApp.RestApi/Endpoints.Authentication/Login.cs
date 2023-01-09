using CustomerApp.Application.Handlers.Authentication;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.Authentication.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Authentication;

[Route($"{BaseUrl}/login")]
public sealed class Login : InternalControllerBase
{
    readonly LoginHandler _commandHandler;

    public Login(LoginHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions) 
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost]
    [Tags(SwaggerTags.Authentication)]
    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(AuthenticationResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<AuthenticationResponse>> Handle(LoginRequest request, CancellationToken ct)
    {
        var command = request.ToCommand();
        var errorOrAuthenticationResult = await _commandHandler.Handle(command, ct);

        return ToActionResult(
            errorOrAuthenticationResult, 
            authenticationResult => Ok(AuthenticationResponse.From(authenticationResult)));
    }
}