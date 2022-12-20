using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Interfaces;

public interface IPasswordHasher
{
    ErrorOr<HashedPassword> Hash(Password password);
    bool IsCorrectPassword(CustomerId customerId, HashedPassword savedPassword, Password password);
}
