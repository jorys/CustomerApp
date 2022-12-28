using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;

public sealed class Token : ValueObject
{
    public string Value { get; }

    Token(string value)
    {
        Value = value;
    }

    public static ErrorOr<Token> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(Token));
        }
        return new Token(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static Token ReloadFromRepository(string value) => new(value);
}
