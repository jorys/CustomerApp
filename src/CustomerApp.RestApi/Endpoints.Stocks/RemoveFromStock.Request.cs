using CustomerApp.Application.Handlers.CustomerStocks.Models;

namespace CustomerApp.RestApi.Endpoints.CustomerStocks;

public sealed record RemoveFromStockRequest
{
    /// <example>book</example>
    public string ItemName { get; }

    /// <example>3</example>
    public int Quantity { get; }

    public RemoveFromStockRequest(string itemName, int quantity)
    {
        ItemName = itemName;
        Quantity = quantity;
    }

    internal RemoveFromStockCommand ToCommand(Guid customerId) =>
        new(
            CustomerId: customerId,
            ItemName: ItemName,
            Quantity: Quantity);
}