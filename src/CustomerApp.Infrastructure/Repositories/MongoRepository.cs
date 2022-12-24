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

    public MongoRepository(IMongoClient mongoClient)
	{
        var customerDb = mongoClient.GetDatabase(CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(_customersCollectionName);
    }

    public Task DeleteCustomer(CustomerId customerId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteResetPasswordResource(CustomerId customerId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesEmailAlreadyExist(Email email)
    {
        // TODO: not working
        var filter = Builders<CustomerBson>.Filter
            .Eq(customer => customer.Email, email.Value);
        var existingCustomer = await _customersCollection.FindAsync(filter);

        return existingCustomer.Current != null;
    }

    public Task<Customer?> GetCustomer(CustomerId customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetCustomer(Email email)
    {
        throw new NotImplementedException();
    }

    public Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId)
    {
        throw new NotImplementedException();
    }

    public Task<ResetPasswordResource?> GetResetPasswordResource(Email email)
    {
        throw new NotImplementedException();
    }

    public Task Save(Customer customer)
    {
        var customerBson = CustomerBson.From(customer);
        return _customersCollection.InsertOneAsync(customerBson);
    }

    public Task Save(LoginAttempt loginAttempt)
    {
        throw new NotImplementedException();
    }

    public Task Save(ResetPasswordResource resetPasswordResource)
    {
        throw new NotImplementedException();
    }
}
