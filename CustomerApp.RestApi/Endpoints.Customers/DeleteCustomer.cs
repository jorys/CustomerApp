using CustomerApp.Application.Handlers.Customers;
using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.RestApi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Customers;

[Route($"{BaseUrl}/customer")]
public sealed class DeleteCustomer : AuthenticatedControllerBase
{
    readonly DeleteCustomerHandler _commandHandler;

    public DeleteCustomer(DeleteCustomerHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions) 
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpDelete]
    [Tags(SwaggerTags.Customer)]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Handle()
    {
        var command = new DeleteCustomerCommand(CustomerId: CustomerId);
        var errorOrSuccess = await _commandHandler.Handle(command);

        return ToActionResult(
            errorOrSuccess, 
            result => NoContent());
    }
}
