using CustomerApp.Domain.Aggregates.CustomerStocks;

namespace CustomerApp.RestApi.Endpoints.CustomerStocks.Responses;

public sealed record StocksResponse
{
    public StockItemResponse[] Stocks { get; }

    public StocksResponse(StockItemResponse[] stocks)
    {
        Stocks = stocks;
    }

    public sealed record StockItemResponse
    {
        /// <example>book</example>
        public string ItemName { get; }

        /// <example>12</example>
        public decimal Quantity { get; }

        public StockItemResponse(string itemName, int quantity)
        {
            ItemName = itemName;
            Quantity = quantity;
        }
    }

    internal static StocksResponse? From(Stocks? stocks)
    {
        if (stocks is null) return null;
        
        return new(
            stocks: stocks.Items.Select(stockItem => 
                new StockItemResponse(
                    itemName: stockItem.Key.Value,
                    quantity: stockItem.Value.Value
            )).ToArray());
    }
}
