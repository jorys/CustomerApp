using CustomerApp.Application.Handlers.Authentication.Interfaces;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
using MongoDB.Driver;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class ResetPasswordMongoRepository : IResetPasswordRepository
{
    readonly IMongoCollection<CustomerBson> _customersCollection;
    readonly IMongoCollection<LoginAttemptBson> _loginAttemptsCollection;
    readonly IMongoCollection<ResetPasswordBson> _resetPasswordsCollection;
    readonly IMongoCollection<StocksBson> _stocksCollection;

    public ResetPasswordMongoRepository(IMongoClient mongoClient)
    {
        var customerDb = mongoClient.GetDatabase(MongoDatabaseNames.CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(MongoDatabaseNames.CustomersCollectionName);
        _loginAttemptsCollection = customerDb.GetCollection<LoginAttemptBson>(MongoDatabaseNames.LoginAttemptsCollectionName);
        _resetPasswordsCollection = customerDb.GetCollection<ResetPasswordBson>(MongoDatabaseNames.ResetPasswordsCollectionName);
        _stocksCollection = customerDb.GetCollection<StocksBson>(MongoDatabaseNames.StocksCollectionName);
    }

    public Task Upsert(ResetPasswordResource resetPasswordResource, CancellationToken ct)
    {
        var resetPasswordBson = ResetPasswordBson.From(resetPasswordResource);
        return _resetPasswordsCollection.ReplaceOneAsync(
            saved => saved.CustomerId == resetPasswordBson.CustomerId,
            resetPasswordBson,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken: ct);
    }

    public async Task<ResetPasswordResource?> GetResetPasswordResource(Email email, CancellationToken ct)
    {
        var resetPasswordsBson = await _resetPasswordsCollection
            .Find(resetPassword => resetPassword.Email == email.Value)
            .SingleOrDefaultAsync(ct);

        if (resetPasswordsBson == null) return null;
        return resetPasswordsBson.ToDomain();
    }

    public Task DeleteResetPasswordResource(CustomerId customerId, CancellationToken ct)
    {
        return _resetPasswordsCollection
            .FindOneAndDeleteAsync(
                resetPassword => resetPassword.CustomerId == customerId.Value,
                cancellationToken: ct);
    }
}
