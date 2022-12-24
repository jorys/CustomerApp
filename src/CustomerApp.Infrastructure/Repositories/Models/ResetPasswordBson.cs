using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CustomerApp.Domain.Aggregates.ResetPasswords;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class ResetPasswordBson
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    internal Guid CustomerId { get; }

    [BsonElement("email")]
    internal string Email{ get; }

    [BsonElement("token")]
    internal string Token { get; }

    [BsonElement("tokenExpiry")]
    internal DateTime TokenExpiry { get; }

    public ResetPasswordBson(Guid customerId, string email, string token, DateTime tokenExpiry)
    {
        CustomerId = customerId;
        Email = email;
        Token = token;
        TokenExpiry = tokenExpiry;
    }

    internal static ResetPasswordBson From(ResetPasswordResource resetPassword) =>
        new(
            customerId: resetPassword.Id.Value,
            email: resetPassword.Email.Value,
            token: resetPassword.Token.Value,
            tokenExpiry: resetPassword.TokenExpiry.Value);
}
