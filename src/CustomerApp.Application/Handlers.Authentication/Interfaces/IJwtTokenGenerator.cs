using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(CustomerId customerId, FirstName firstName, LastName lastName);
}
