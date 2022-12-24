using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;

public sealed class TokenExpiry : ValueObject
{
    public DateTime Value { get; }

    TokenExpiry(DateTime value)
    {
        Value = value;
    }

    public static ErrorOr<TokenExpiry> Create(DateTime value)
    {
        if (value <= DateTime.UtcNow)
        {
            return Errors.DateShouldBeInFuture(nameof(TokenExpiry));
        }
        return new TokenExpiry(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal bool IsExpired() => DateTime.UtcNow > Value;
}
