using CustomerApp.Application.Handlers.Authentication;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.Authentication.Responses;
using CustomerApp.RestApi.Endpoints.Customers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Authentication;

[ApiController]
[Route("api/register")]
public sealed class Register : InternalControllerBase
{
    readonly RegisterHandler _commandHandler;

    public Register(RegisterHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions) 
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost]
    [Tags("Authentication")]
    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(AuthenticationResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<AuthenticationResponse>> Handle(RegisterRequest request)
    {
        var command = request.ToCommand();
        var errorOrAuthenticationResult = await _commandHandler.Handle(command);

        return ToActionResult(
            errorOrAuthenticationResult,
            authenticationResult => Ok(AuthenticationResponse.From(authenticationResult)));
    }
}