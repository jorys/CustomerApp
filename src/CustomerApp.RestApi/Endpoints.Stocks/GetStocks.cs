using CustomerApp.Application.Handlers.CustomerStocks;
using CustomerApp.Application.Handlers.CustomerStocks.Models;
using CustomerApp.RestApi.Common;
using CustomerApp.RestApi.Endpoints.CustomerStocks.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CustomerApp.RestApi.Endpoints.CustomerStocks;

[Route($"{BaseUrl}/stocks")]
public class GetStocks : AuthenticatedControllerBase
{
    readonly GetStocksHandler _queryHandler;

    public GetStocks(GetStocksHandler queryHandler, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        : base(apiBehaviorOptions)
    {
        _queryHandler = queryHandler;
    }

    [HttpGet]
    [Tags(SwaggerTags.Stock)]
    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(StocksResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<StocksResponse>> Handle(CancellationToken ct)
    {
        var query = new GetStocksQuery(CustomerId);
        var errorOrStocks = await _queryHandler.Handle(query, ct);

        return ToActionResult(
            errorOrStocks, 
            stocks => Ok(StocksResponse.From(stocks)));
    }
}