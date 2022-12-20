using CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;
using CustomerApp.Domain.Common.ValueObjects;

namespace CustomerApp.Application.Interfaces;

public interface IEmailSender
{
    Task<bool> Send(Email email, Token token);
}
