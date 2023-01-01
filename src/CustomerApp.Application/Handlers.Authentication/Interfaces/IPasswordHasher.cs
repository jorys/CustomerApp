using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Interfaces;

public interface IPasswordHasher
{
    string Hash(Password password);
    bool IsCorrectPassword(CustomerId customerId, HashedPassword savedPassword, Password password);
}
