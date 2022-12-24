using CustomerApp.Application.Handlers.Customers;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.Customers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Customers;

[Route($"{BaseUrl}/customer")]
public class UpdateCustomer : AuthenticatedControllerBase
{
    readonly UpdateCustomerHandler _commandHandler;

    public UpdateCustomer(UpdateCustomerHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPatch]
    [Tags(SwaggerTags.Customer)]
    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(CustomerResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<CustomerResponse>> Handle(UpdateCustomerRequest request)
    {
        var command = request.ToCommand(CustomerId);
        var errorOrCustomer = await _commandHandler.Handle(command);

        return ToActionResult(
            errorOrCustomer, 
            customer => Ok(CustomerResponse.From(customer)));
    }
}