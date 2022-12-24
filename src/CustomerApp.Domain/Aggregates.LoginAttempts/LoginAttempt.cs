using CustomerApp.Domain.Aggregates.LoginAttempts.ValueObjects;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Domain.LoginAttemptAggregate;

public sealed class LoginAttempt : AggregateRoot<CustomerId>
{
    public override CustomerId Id { get; }
    public AttemptDate LastAttemptDate { get; private set; }
    public AttemptStatus AttemptStatus { get; private set; }
    public AttemptCount AttemptCount { get; private set; }

    LoginAttempt(CustomerId customerId, AttemptDate lastAttemptDate, AttemptStatus attemptStatus, AttemptCount attemptCount) : base(customerId)
    {
        Id = customerId;
        LastAttemptDate = lastAttemptDate;
        AttemptStatus = attemptStatus;
        AttemptCount = attemptCount;
    }

    public static ErrorOr<LoginAttempt> CreateSuccess(CustomerId customerId)
    {
        if (customerId is null)
        {
            return Errors.IsRequired(nameof(customerId));
        }
        return new LoginAttempt(
            customerId,
            AttemptDate.Create(),
            AttemptStatus.CreateSuccess(),
            AttemptCount.Create());
    }

    public static LoginAttempt CreateFailed(CustomerId customerId)
    {
        return new LoginAttempt(
            customerId,
            AttemptDate.Create(),
            AttemptStatus.CreateFailed(),
            AttemptCount.Create());
    }

    public ErrorOr<LoginAttempt> AttemptFails()
    {
        var errorOrIncrementedAttemptCount = AttemptCount.Increment();
        if (errorOrIncrementedAttemptCount.IsError) return errorOrIncrementedAttemptCount.Errors;

        AttemptStatus = AttemptStatus.CreateFailed();
        LastAttemptDate = AttemptDate.Create();
        AttemptCount = errorOrIncrementedAttemptCount.Value;

        return this;
    }
}
