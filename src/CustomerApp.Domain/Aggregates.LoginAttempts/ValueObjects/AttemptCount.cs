using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.LoginAttempts.ValueObjects;

public sealed class AttemptCount : ValueObject
{
    private const int _maxRetryCount = 5;

    public int Value { get; }

    AttemptCount(int value)
    {
        Value = value;
    }

    internal static AttemptCount Create()
    {
        return new AttemptCount(1);
    }

    internal ErrorOr<AttemptCount> Increment()
    {
        if (Value >= _maxRetryCount)
        {
            return Errors.MaximumLoginAttempt();
        }
        return new AttemptCount(Value + 1);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static AttemptCount ReloadFromRepository(int value) => new(value);
}
