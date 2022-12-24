using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
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
            .FindOneAndDeleteAsync(customer => customer.CustomerId == customerId.Value);
    }

    public Task DeleteResetPasswordResource(CustomerId customerId, CancellationToken ct)
    {
        return _resetPasswordsCollection
            .FindOneAndDeleteAsync(resetPassword => resetPassword.CustomerId == customerId.Value);
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
            .SingleOrDefaultAsync();

        if (customerBson == null) return null;
        return customerBson.ToDomain();
    }

    public async Task<Customer?> GetCustomer(Email email, CancellationToken ct)
    {
        var customerBson = await _customersCollection
            .Find(customer => customer.Email == email.Value)
            .SingleOrDefaultAsync();

        if (customerBson == null) return null;
        return customerBson.ToDomain();
    }

    public async Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId, CancellationToken ct)
    {
        var loginAttemptsBson = await _loginAttemptsCollection
            .Find(loginAttempt => loginAttempt.CustomerId == customerId.Value)
            .SingleOrDefaultAsync();

        if (loginAttemptsBson == null) return null;
        return loginAttemptsBson.ToDomain();
    }

    public async Task<ResetPasswordResource?> GetResetPasswordResource(Email email, CancellationToken ct)
    {
        var resetPasswordsBson = await _resetPasswordsCollection
            .Find(resetPassword => resetPassword.Email == email.Value)
            .SingleOrDefaultAsync();

        if (resetPasswordsBson == null) return null;
        return resetPasswordsBson.ToDomain();
    }

    public Task Save(Customer customer, CancellationToken ct)
    {
        var customerBson = CustomerBson.From(customer);
        return _customersCollection.InsertOneAsync(customerBson, cancellationToken: ct);
    }

    public Task Save(LoginAttempt loginAttempt, CancellationToken ct)
    {
        var loginAttemptBson = LoginAttemptBson.From(loginAttempt);
        return _loginAttemptsCollection.InsertOneAsync(loginAttemptBson, cancellationToken: ct);
    }

    public Task Save(ResetPasswordResource resetPasswordResource, CancellationToken ct)
    {
        var resetPassordBson = ResetPasswordBson.From(resetPasswordResource);
        return _resetPasswordsCollection.InsertOneAsync(resetPassordBson, cancellationToken: ct);
    }
}
