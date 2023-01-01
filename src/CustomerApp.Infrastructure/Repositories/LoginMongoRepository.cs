﻿using CustomerApp.Application.Handlers.Authentication.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
using MongoDB.Driver;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class LoginMongoRepository : ILoginRepository
{
    readonly IMongoCollection<CustomerBson> _customersCollection;
    readonly IMongoCollection<LoginAttemptBson> _loginAttemptsCollection;

    public LoginMongoRepository(IMongoClient mongoClient)
    {
        var customerDb = mongoClient.GetDatabase(MongoDatabaseNames.CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(MongoDatabaseNames.CustomersCollectionName);
        _loginAttemptsCollection = customerDb.GetCollection<LoginAttemptBson>(MongoDatabaseNames.LoginAttemptsCollectionName);
    }

    public async Task<Customer?> GetCustomer(Email email, CancellationToken ct)
    {
        var customerBson = await _customersCollection
            .Find(customer => customer.Email == email.Value)
            .SingleOrDefaultAsync(ct);

        if (customerBson == null) return null;
        return customerBson.ToDomain();
    }

    public async Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId, CancellationToken ct)
    {
        var loginAttemptsBson = await _loginAttemptsCollection
            .Find(loginAttempt => loginAttempt.CustomerId == customerId.Value)
            .SingleOrDefaultAsync(ct);

        if (loginAttemptsBson == null) return null;
        return loginAttemptsBson.ToDomain();
    }

    public Task Upsert(LoginAttempt loginAttempt, CancellationToken ct)
    {
        var loginAttemptBson = LoginAttemptBson.From(loginAttempt);
        return _loginAttemptsCollection.ReplaceOneAsync(
            saved => saved.CustomerId == loginAttemptBson.CustomerId,
            loginAttemptBson,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken: ct);
    }
}
