namespace CustomerApp.Application.Handlers.CustomerStocks.Models;

public record AddToStockCommand(
    Guid CustomerId,
    string ItemName,
    int Quantity);