using CustomerApp.Application.Handlers.CustomerStocks.Interfaces;
using CustomerApp.Application.Handlers.CustomerStocks.Settings;
using CustomerApp.Application.Handlers.CustomerStocks.Models;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Options;
using CustomerApp.Domain.Aggregates.CustomerStocks;
using CustomerApp.Domain.Common;

namespace CustomerApp.Application.Handlers.CustomerStocks;

public sealed class AddToStockHandler
{
    readonly IStocksRepository _repository;
    readonly ConcurrencyRetrySettings _settings;

    public AddToStockHandler(IStocksRepository repository, IOptions<ConcurrencyRetrySettings> options)
    {
        _repository = repository;
        _settings = options.Value;
    }

    public async Task<ErrorOr<Stocks>> Handle(AddToStockCommand command, CancellationToken ct, int tryCount = 1)
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

        // To test concurrency, add delay
        //await Task.Delay(3000);

        // Create new if not exist
        if (stocks is null)
        {
            return await InsertStocks(customerId, command, ct);
        }

        // Keep previous version
        var previousVersion = stocks.Version;

        // Update
        var errorOrStocks = stocks.AddToStock(command.ItemName, command.Quantity);
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

    async Task<ErrorOr<Stocks>> InsertStocks(CustomerId customerId, AddToStockCommand command, CancellationToken ct)
    {
        var errorOrStocks = Stocks
            .Create(customerId)
            .AddToStock(itemName: command.ItemName,
            quantityToAdd: command.Quantity);

        if (errorOrStocks.IsError) return errorOrStocks.Errors;
        var stocks = errorOrStocks.Value;

        await _repository.InsertStocks(stocks, ct);
        return stocks;
    }
}
