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

    public static AttemptCount Create()
    {
        return new AttemptCount(1);
    }

    public ErrorOr<AttemptCount> Increment()
    {
        if (Value >= _maxRetryCount)
        {
            return Errors.MaximumLoginAttempt();
        }
        return new AttemptCount(Value + 1);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
