using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Interfaces;

public interface IRepository
{
    Task<Customer?> GetCustomer(CustomerId customerId);
    Task<Customer?> GetCustomer(Email email);
    Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId);

    Task<bool> DoesEmailAlreadyExist(Email email);

    Task Save(Customer customer);
    Task Save(LoginAttempt loginAttempt);
    Task Save(ResetPasswordResource resetPasswordResource);
    Task DeleteCustomer(CustomerId customerId);
    Task DeleteResetPasswordResource(CustomerId customerId);
    Task<ResetPasswordResource?> GetResetPasswordResource(Email email);
}
