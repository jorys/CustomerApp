using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class MongoRepository : IRepository
{
    internal const string CustomerDb = "customer";
    
    const string _customersCollectionName = "customers";
    const string _loginAttemptsCollectionName = "loginattempts";
    const string _resetPasswordsCollectionName = "resetpasswords";

    readonly IMongoCollection<CustomerBson> _customersCollection;
    readonly IMongoCollection<LoginAttemptBson> _loginAttemptsCollection;
    readonly IMongoCollection<ResetPasswordBson> _resetPasswordsCollection;

    public MongoRepository(IMongoClient mongoClient)
    {
        var customerDb = mongoClient.GetDatabase(CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(_customersCollectionName);
        _loginAttemptsCollection = customerDb.GetCollection<LoginAttemptBson>(_loginAttemptsCollectionName);
        _resetPasswordsCollection = customerDb.GetCollection<ResetPasswordBson>(_resetPasswordsCollectionName);
    }

    public Task DeleteCustomer(CustomerId customerId, CancellationToken ct)
    {
        return _customersCollection
            .FindOneAndDeleteAsync(
                customer => customer.CustomerId == customerId.Value,
                cancellationToken: ct);
    }

    public Task DeleteResetPasswordResource(CustomerId customerId, CancellationToken ct)
    {
        return _resetPasswordsCollection
            .FindOneAndDeleteAsync(
                resetPassword => resetPassword.CustomerId == customerId.Value,
                cancellationToken: ct);
    }

    public Task<bool> DoesEmailAlreadyExist(Email email, CancellationToken ct)
    {
        return _customersCollection
            .Find(customer => customer.Email == email.Value)
            .AnyAsync(ct);
    }

    public async Task<Customer?> GetCustomer(CustomerId customerId, CancellationToken ct)
    {
        var customerBson = await _customersCollection
            .Find(customer => customer.CustomerId == customerId.Value)
            .SingleOrDefaultAsync(ct);

        if (customerBson == null) return null;
        return customerBson.ToDomain();
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

    public async Task<ResetPasswordResource?> GetResetPasswordResource(Email email, CancellationToken ct)
    {
        var resetPasswordsBson = await _resetPasswordsCollection
            .Find(resetPassword => resetPassword.Email == email.Value)
            .SingleOrDefaultAsync(ct);

        if (resetPasswordsBson == null) return null;
        return resetPasswordsBson.ToDomain();
    }

    // To manage concurrency, insert only if email does not exist
    public async Task<bool> Insert(Customer customer, CancellationToken ct)
    {
        var updateResult = await _customersCollection.UpdateOneAsync(
            saved => saved.Email == customer.Email.Value,
            Builders<CustomerBson>.Update
                .SetOnInsert(c => c.CustomerId, customer.Id.Value)
                .SetOnInsert(c => c.FirstName, customer.FirstName.Value)
                .SetOnInsert(c => c.LastName, customer.LastName.Value)
                .SetOnInsert(c => c.Birthdate, customer.Birthdate.Value)
                .SetOnInsert(c => c.Email, customer.Email.Value)
                .SetOnInsert(c => c.HashedPassword, customer.HashedPassword.Value)
                .SetOnInsert(c => c.CustomerStatus, customer.Status.Value)
                .SetOnInsert(c => c.Address.Street, customer.Address.Street)
                .SetOnInsert(c => c.Address.City, customer.Address.City)
                .SetOnInsert(c => c.Address.PostCode, customer.Address.PostCode)
                .SetOnInsert(c => c.Address.Country, customer.Address.Country),
            new UpdateOptions { IsUpsert = true },
            cancellationToken: ct);

        return updateResult.MatchedCount == 0;
    }

    public Task Update(Customer customer, CancellationToken ct)
    {
        var customerBson = CustomerBson.From(customer);
        return _customersCollection.ReplaceOneAsync(
            saved => saved.CustomerId == customerBson.CustomerId,
            customerBson, 
            cancellationToken: ct);
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

    public Task Upsert(ResetPasswordResource resetPasswordResource, CancellationToken ct)
    {
        var resetPasswordBson = ResetPasswordBson.From(resetPasswordResource);
        return _resetPasswordsCollection.ReplaceOneAsync(
            saved => saved.CustomerId == resetPasswordBson.CustomerId,
            resetPasswordBson,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken: ct);
    }
}
