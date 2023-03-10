using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CustomerApp.Domain.Aggregates.ResetPasswords;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class ResetPasswordBson
{
    [BsonId]
    internal Guid CustomerId { get; init; }

    [BsonElement("email")]
    internal string Email{ get; init; }

    [BsonElement("token")]
    internal string Token { get; init; }

    [BsonElement("tokenExpiry")]
    internal DateTime TokenExpiry { get; init; }

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

    internal ResetPasswordResource ToDomain() =>
        ResetPasswordResource.ReloadFromRepository(
            customerId: CustomerId,
            email: Email,
            token: Token,
            tokenExpiry: TokenExpiry);
}
