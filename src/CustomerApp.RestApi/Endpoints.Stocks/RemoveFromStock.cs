using CustomerApp.Application.Handlers.CustomerStocks;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.CustomerStocks.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.CustomerStocks;

[Route($"{BaseUrl}/stocks/remove-items")]
public class RemoveFromStock : AuthenticatedControllerBase
{
    readonly RemoveFromStockHandler _commandHandler;

    public RemoveFromStock(RemoveFromStockHandler commandHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        : base(apiBehaviorOptions)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost]
    [Tags(SwaggerTags.Stock)]
    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(StocksResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<StocksResponse>> Handle(RemoveFromStockRequest request, CancellationToken ct)
    {
        var command = request.ToCommand(CustomerId);
        var errorOrStocks = await _commandHandler.Handle(command, ct);

        return ToActionResult(
            errorOrStocks, 
            stocks => Ok(StocksResponse.From(stocks)));
    }
}