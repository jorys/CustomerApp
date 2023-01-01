using CustomerApp.Application.Handlers.CustomerStocks.Interfaces;
using CustomerApp.Domain.Aggregates.CustomerStocks;
using CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
using MongoDB.Driver;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class StocksMongoRepository : IStocksRepository
{
    readonly IMongoCollection<CustomerBson> _customersCollection;
    readonly IMongoCollection<StocksBson> _stocksCollection;

    public StocksMongoRepository(IMongoClient mongoClient)
    {
        var customerDb = mongoClient.GetDatabase(MongoDatabaseNames.CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(MongoDatabaseNames.CustomersCollectionName);
        _stocksCollection = customerDb.GetCollection<StocksBson>(MongoDatabaseNames.StocksCollectionName);
    }

    public Task<bool> DoesCustomerExist(CustomerId customerId, CancellationToken ct)
    {
        return _customersCollection
            .Find(customer => customer.CustomerId == customerId.Value)
            .AnyAsync(ct);
    }

    public async Task<Stocks?> GetStocks(CustomerId customerId, CancellationToken ct)
    {
        var stocksBson = await _stocksCollection
            .Find(customer => customer.CustomerId == customerId.Value)
            .SingleOrDefaultAsync(ct);

        if (stocksBson == null) return null;
        return stocksBson.ToDomain();
    }

    // Concurrency managed by collection identifier customerId
    public Task InsertStocks(Stocks stocks, CancellationToken ct)
    {
        var stocksBson = StocksBson.From(stocks);
        return _stocksCollection.InsertOneAsync(stocksBson, cancellationToken: ct);
    }

    // To manage concurrency, check aggregate version
    public async Task<bool> Update(Stocks stocks, StocksVersion previousVersion, CancellationToken ct)
    {
        var stocksBson = StocksBson.From(stocks);
        var replaceOneResult = await _stocksCollection.ReplaceOneAsync(
            saved =>
                saved.CustomerId == stocksBson.CustomerId &&
                saved.StocksVersion == previousVersion.Value,
            stocksBson,
            cancellationToken: ct);

        if (replaceOneResult.ModifiedCount == 0) return false;
        return true;
    }
}
