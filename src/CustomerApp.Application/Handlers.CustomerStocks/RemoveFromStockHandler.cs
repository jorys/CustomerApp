using CustomerApp.Application.Handlers.CustomerStocks.Interfaces;
using CustomerApp.Application.Handlers.CustomerStocks.Settings;
using CustomerApp.Application.Handlers.CustomerStocks.Models;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Options;
using CustomerApp.Domain.Aggregates.CustomerStocks;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;

namespace CustomerApp.Application.Handlers.CustomerStocks;

public sealed class RemoveFromStockHandler
{
    readonly IStocksRepository _repository;
    readonly ConcurrencyRetrySettings _settings;

    public RemoveFromStockHandler(IStocksRepository repository, IOptions<ConcurrencyRetrySettings> options)
    {
        _repository = repository;
        _settings = options.Value;
    }

    public async Task<ErrorOr<Stocks>> Handle(RemoveFromStockCommand command, CancellationToken ct, int tryCount = 1)
    {
        // Create customerId
        var errorOrCustomerId = CustomerId.Create(command.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        // Check if customerId exists
        var customerId = errorOrCustomerId.Value;
        var customerIdExists = await _repository.DoesCustomerExist(customerId, ct);
        if (!customerIdExists) return Errors.NotFound("Customer");

        // Get saved stocks
        var stocks = await _repository.GetStocks(customerId, ct);

        // Not enough stock
        if (stocks is null) return Errors.CannotBeNegative(nameof(ItemQuantity));

        // Keep previous version
        var previousVersion = stocks.Version;

        // Update
        var errorOrStocks = stocks.RemoveFromStock(command.ItemName, command.Quantity);
        if (errorOrStocks.IsError) return errorOrStocks.Errors;

        // Insert into
        var isSuccess = await _repository.Update(errorOrStocks.Value, previousVersion, ct);
        if (isSuccess) return stocks;

        // Retry on failure
        if (tryCount <= _settings.MaxTryCount)
        {
            return await Handle(command: command, ct: ct, tryCount: tryCount + 1);
        }
        else
        {
            return Errors.MaxConcurrencyRetries("Stocks");
        }
    }
}
