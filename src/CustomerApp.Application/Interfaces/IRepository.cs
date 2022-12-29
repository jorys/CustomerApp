using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Interfaces;

public interface IRepository
{
    Task<Customer?> GetCustomer(CustomerId customerId, CancellationToken ct);
    Task<Customer?> GetCustomer(Email email, CancellationToken ct);
    Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId, CancellationToken ct);

    Task<bool> DoesEmailAlreadyExist(Email email, CancellationToken ct);

    Task Save(Customer customer, CancellationToken ct);
    Task Save(LoginAttempt loginAttempt, CancellationToken ct);
    Task Save(ResetPasswordResource resetPasswordResource, CancellationToken ct);
    Task DeleteCustomer(CustomerId customerId, CancellationToken ct);
    Task DeleteResetPasswordResource(CustomerId customerId, CancellationToken ct);
    Task<ResetPasswordResource?> GetResetPasswordResource(Email email, CancellationToken ct);
}
