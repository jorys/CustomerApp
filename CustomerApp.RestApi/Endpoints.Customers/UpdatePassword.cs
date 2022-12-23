using CustomerApp.Application.Handlers.Customers;
using CustomerApp.RestApi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Customers;

[Route($"{BaseUrl}/customer/password")]
public class UpdatePassword : AuthenticatedControllerBase
{
    readonly UpdatePasswordHandler _commandHandler;

    public UpdatePassword(UpdatePasswordHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPut]
    [Tags(SwaggerTags.Customer)]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Handle(UpdatePasswordRequest request)
    {
        var command = request.ToCommand(CustomerId);
        var errorOrSuccess = await _commandHandler.Handle(command);

        return ToActionResult(
            errorOrSuccess, 
            success => NoContent());
    }
}