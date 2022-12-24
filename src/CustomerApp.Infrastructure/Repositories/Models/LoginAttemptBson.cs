using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CustomerApp.Domain.LoginAttemptAggregate;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class LoginAttemptBson
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    internal Guid CustomerId { get; }

    [BsonElement("lastAttemptDate")]
    internal DateTime LastAttemptDate { get; }

    [BsonElement("status")]
    internal string AttemptStatus { get; }

    [BsonElement("attemptCount")]
    internal int AttemptCount { get; }

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
}
