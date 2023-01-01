using CustomerApp.Domain.Aggregates.CustomerStocks;
using CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.CustomerStocks.Interfaces;

public interface IStocksRepository
{
    Task<bool> DoesCustomerExist(CustomerId customerId, CancellationToken ct);
    Task<Stocks?> GetStocks(CustomerId customerId, CancellationToken ct);
    Task<bool> InsertStocks(Stocks stocks, CancellationToken ct);
    Task<bool> Update(Stocks stocks, StocksVersion previousVersion, CancellationToken ct);
}
