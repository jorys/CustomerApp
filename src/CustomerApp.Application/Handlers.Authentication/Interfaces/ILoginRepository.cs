using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Interfaces;

public interface ILoginRepository
{
    Task<Customer?> GetCustomer(Email email, CancellationToken ct);
    Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId, CancellationToken ct);
    Task Upsert(LoginAttempt loginAttempt, CancellationToken ct);
}
