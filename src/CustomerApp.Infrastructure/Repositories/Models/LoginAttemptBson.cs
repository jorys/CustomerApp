using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CustomerApp.Domain.LoginAttemptAggregate;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class LoginAttemptBson
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    internal Guid CustomerId { get; init; }

    [BsonElement("lastAttemptDate")]
    internal DateTime LastAttemptDate { get; init; }

    [BsonElement("status")]
    internal string AttemptStatus { get; init; }

    [BsonElement("attemptCount")]
    internal int AttemptCount { get; init; }

    public LoginAttemptBson(Guid customerId, DateTime lastAttemptDate, string attemptStatus, int attemptCount)
    {
        CustomerId = customerId;
        LastAttemptDate = lastAttemptDate;
        AttemptStatus = attemptStatus;
        AttemptCount = attemptCount;
    }

    internal static LoginAttemptBson From(LoginAttempt loginAttempt) =>
        new(
            customerId: loginAttempt.Id.Value,
            lastAttemptDate: loginAttempt.LastAttemptDate.Value,
            attemptStatus: loginAttempt.AttemptStatus.Value,
            attemptCount: loginAttempt.AttemptCount.Value);

    internal LoginAttempt ToDomain()
    {
        // TODO
        throw new NotImplementedException();
    }
}
