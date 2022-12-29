using CustomerApp.Application.Handlers.Customers;
using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.Customers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.Customers;

[Route($"{BaseUrl}/customer")]
public sealed class GetCustomer : AuthenticatedControllerBase
{
    readonly GetCustomerHandler _queryHandler;

    public GetCustomer(GetCustomerHandler queryHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions) 
        : base(apiBehaviorOptions)
    {
        _queryHandler = queryHandler;
    }

    [HttpGet]
    [Tags(SwaggerTags.Customer)]
    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(CustomerResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<CustomerResponse>> Handle(CancellationToken ct)
    {
        var query = new GetCustomerQuery(CustomerId: CustomerId);
        var errorOrCustomer = await _queryHandler.Handle(query, ct);

        return ToActionResult(
            errorOrCustomer, 
            customer => Ok(CustomerResponse.From(customer)));
    }
}