using CustomerApp.Application.Handlers.CustomerStocks.Models;

namespace CustomerApp.RestApi.Endpoints.CustomerStocks;

public sealed record AddToStockRequest
{
    /// <example>book</example>
    public string ItemName { get; }

    /// <example>12</example>
    public int Quantity { get; }

    public AddToStockRequest(string itemName, int quantity)
    {
        ItemName = itemName;
        Quantity = quantity;
    }

    internal AddToStockCommand ToCommand(Guid customerId) =>
        new(
            CustomerId: customerId,
            ItemName: ItemName,
            Quantity: Quantity);
}