namespace CustomerApp.Application.Handlers.Customers.Models;

public record DeleteCustomerCommand(
    Guid CustomerId);
