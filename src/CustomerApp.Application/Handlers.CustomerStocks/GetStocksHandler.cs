using CustomerApp.Application.Handlers.CustomerStocks.Interfaces;
using CustomerApp.Application.Handlers.CustomerStocks.Settings;
using CustomerApp.Application.Handlers.CustomerStocks.Models;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Options;
using CustomerApp.Domain.Aggregates.CustomerStocks;

namespace CustomerApp.Application.Handlers.CustomerStocks;

public sealed class GetStocksHandler
{
    readonly IStocksRepository _repository;

    public GetStocksHandler(IStocksRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Stocks?>> Handle(GetStocksQuery query, CancellationToken ct)
    {
        // Create customerId
        var errorOrCustomerId = CustomerId.Create(query.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;
        var customerId = errorOrCustomerId.Value;

        // Get saved stocks
        return await _repository.GetStocks(customerId, ct);
    }
}
