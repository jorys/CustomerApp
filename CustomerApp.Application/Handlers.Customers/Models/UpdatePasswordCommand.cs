namespace CustomerApp.Application.Handlers.Customers.Models;

public record UpdatePasswordCommand(
    Guid CustomerId,
    string Password);