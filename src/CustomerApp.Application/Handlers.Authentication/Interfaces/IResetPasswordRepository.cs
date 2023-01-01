using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Interfaces;

public interface IResetPasswordRepository
{
    Task Upsert(ResetPasswordResource resetPasswordResource, CancellationToken ct);
    Task<ResetPasswordResource?> GetResetPasswordResource(Email email, CancellationToken ct);
    Task DeleteResetPasswordResource(CustomerId customerId, CancellationToken ct);
}
