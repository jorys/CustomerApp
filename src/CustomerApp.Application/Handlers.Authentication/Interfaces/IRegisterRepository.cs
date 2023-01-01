using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Common.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Interfaces;

public interface IRegisterRepository
{
    Task<bool> DoesEmailAlreadyExist(Email email, CancellationToken ct);
    Task<bool> Insert(Customer customer, CancellationToken ct);
}
