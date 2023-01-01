﻿using CustomerApp.Domain.Aggregates.CustomerStocks;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class StocksBson
{
    [BsonId]
    internal Guid CustomerId { get; init; }

    [BsonElement("version")]
    internal Guid StocksVersion { get; init; }

    [BsonElement("stocks")]
    internal StockItemBson[] StockItems { get; init; }

    public StocksBson(Guid customerId, Guid stocksVersion, StockItemBson[] stockItems)
    {
        CustomerId = customerId;
        StocksVersion = stocksVersion;
        StockItems = stockItems;
    }

    internal static StocksBson From(Stocks stocks) =>
        new(
            customerId: stocks.Id.Value,
            stocksVersion: stocks.Version.Value,
            stockItems: stocks.Items.Select(stockItem =>
                new StockItemBson(
                    name: stockItem.Key.Value,
                    quantity: stockItem.Value.Value
                    )).ToArray());

    internal Stocks ToDomain(StocksBson stocks) =>
        Stocks.ReloadFromRepository(
            customerId: stocks.CustomerId,
            stocksVersion: stocks.StocksVersion,
            stockItems: stocks.StockItems.ToDictionary(
                stockItem => stockItem.Name,
                stockItem => stockItem.Quantity));
}
