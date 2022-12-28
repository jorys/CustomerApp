using CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.ResetPasswords;

public sealed class ResetPasswordResource : AggregateRoot<CustomerId>
{
    public override CustomerId Id { get; }
    public Email Email { get; }
    public Token Token { get; }
    public TokenExpiry TokenExpiry { get; }

    ResetPasswordResource(CustomerId customerId, Email email, Token resetPasswordToken, TokenExpiry resetPasswordTokenExpiry) : base(customerId)
    {
        Id = customerId;
        Email = email;
        Token = resetPasswordToken;
        TokenExpiry = resetPasswordTokenExpiry;
    }

    public static ErrorOr<ResetPasswordResource> Create(CustomerId customerId, Email email, Token token, TokenExpiry tokenExpiry)
    {
        var errors = new List<Error>(4);
        if (customerId is null)
        {
            errors.Add(Errors.IsRequired(nameof(CustomerId)));
        }
        if (email is null)
        {
            errors.Add(Errors.IsRequired(nameof(Email)));
        }
        if (token is null)
        {
            errors.Add(Errors.IsRequired(nameof(ValueObjects.Token)));
        }
        if (tokenExpiry is null)
        {
            errors.Add(Errors.IsRequired(nameof(ValueObjects.TokenExpiry)));
        }
        if (errors.Any()) return errors;

        return new ResetPasswordResource(customerId, email, token, tokenExpiry);
    }

    public bool IsTokenExpired() => TokenExpiry.IsExpired();

    public static ResetPasswordResource ReloadFromRepository(Guid customerId, string email, string token, DateTime tokenExpiry) =>
        new(
            CustomerId.ReloadFromRepository(customerId),
            Email.ReloadFromRepository(email),
            Token.ReloadFromRepository(token),
            TokenExpiry.ReloadFromRepository(tokenExpiry));
}
