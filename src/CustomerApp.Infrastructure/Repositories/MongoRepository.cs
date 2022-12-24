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
    readonly IMongoCollection<LoginAttemptBson> _loginAttemptCollection;
    readonly IMongoCollection<ResetPasswordBson> _resetPasswordsCollection;

    public MongoRepository(IMongoClient mongoClient)
	{
        var customerDb = mongoClient.GetDatabase(CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(_customersCollectionName);
        _loginAttemptCollection = customerDb.GetCollection<LoginAttemptBson>(_loginAttemptsCollectionName);
        _resetPasswordsCollection = customerDb.GetCollection<ResetPasswordBson>(_resetPasswordsCollectionName);
    }

    public Task DeleteCustomer(CustomerId customerId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task DeleteResetPasswordResource(CustomerId customerId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesEmailAlreadyExist(Email email, CancellationToken ct)
    {
        // TODO: not working
        var filter = Builders<CustomerBson>.Filter
            .Eq(customer => customer.Email, email.Value);
        var existingCustomer = await _customersCollection.FindAsync(filter, cancellationToken: ct);

        return existingCustomer.Current != null;
    }

    public Task<Customer?> GetCustomer(CustomerId customerId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetCustomer(Email email, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ResetPasswordResource?> GetResetPasswordResource(Email email, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task Save(Customer customer, CancellationToken ct)
    {
        // TODO: insert if not exists
        var customerBson = CustomerBson.From(customer);
        return _customersCollection.InsertOneAsync(customerBson, cancellationToken: ct);
    }

    public Task Save(LoginAttempt loginAttempt, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task Save(ResetPasswordResource resetPasswordResource, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
