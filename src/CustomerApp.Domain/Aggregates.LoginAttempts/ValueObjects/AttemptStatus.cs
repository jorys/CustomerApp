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

    public static AttemptStatus CreateSuccess()
    {
        return new AttemptStatus(Success);
    }

    public static AttemptStatus CreateFailed()
    {
        return new AttemptStatus(Failed);
    }

    public bool IsSuccess()
    {
        return Value == Success;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static AttemptStatus ReloadFromRepository(string value) => new(value);
}
