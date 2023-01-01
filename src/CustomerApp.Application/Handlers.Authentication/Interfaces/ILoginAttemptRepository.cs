using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Interfaces;

public interface ILoginAttemptRepository
{
    Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId, CancellationToken ct);
    Task Upsert(LoginAttempt loginAttempt, CancellationToken ct);
}
