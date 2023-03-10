using CustomerApp.Domain.Common;

namespace CustomerApp.Domain.Aggregates.LoginAttempts.ValueObjects;

public sealed class AttemptStatus : ValueObject
{
    const string Success = "success";
    const string Failed = "failed";

    public string Value { get; }

    AttemptStatus(string value)
    {
        Value = value;
    }

    internal static AttemptStatus CreateSuccess() => new(Success);

    internal static AttemptStatus CreateFailed() => new(Failed);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static AttemptStatus ReloadFromRepository(string value) => new(value);
}
