using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(Password password);
    bool IsCorrectPassword(CustomerId customerId, HashedPassword savedPassword, Password password);
}
