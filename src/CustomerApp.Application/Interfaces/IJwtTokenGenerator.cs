using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(CustomerId customerId, FirstName firstName, LastName lastName);
}
