namespace CustomerApp.Application.Handlers.CustomerStocks.Models;

public record RemoveFromStockCommand(
    Guid CustomerId,
    string ItemName,
    int Quantity);