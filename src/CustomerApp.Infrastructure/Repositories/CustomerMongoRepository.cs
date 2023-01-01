using CustomerApp.Application.Handlers.Customers.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
using MongoDB.Driver;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class CustomerMongoRepository : ICustomerRepository
{
    readonly IMongoCollection<CustomerBson> _customersCollection;

    public CustomerMongoRepository(IMongoClient mongoClient)
    {
        var customerDb = mongoClient.GetDatabase(MongoDatabaseNames.CustomerDb);
        _customersCollection = customerDb.GetCollection<CustomerBson>(MongoDatabaseNames.CustomersCollectionName);
    }

    public async Task<Customer?> GetCustomer(CustomerId customerId, CancellationToken ct)
    {
        var customerBson = await _customersCollection
            .Find(customer => customer.CustomerId == customerId.Value)
            .SingleOrDefaultAsync(ct);

        if (customerBson == null) return null;
        return customerBson.ToDomain();
    }

    public Task Update(Customer customer, CancellationToken ct)
    {
        var customerBson = CustomerBson.From(customer);
        return _customersCollection.ReplaceOneAsync(
            saved => saved.CustomerId == customerBson.CustomerId,
            customerBson,
            cancellationToken: ct);
    }
    public Task DeleteCustomer(CustomerId customerId, CancellationToken ct)
    {
        return _customersCollection
            .FindOneAndDeleteAsync(
                customer => customer.CustomerId == customerId.Value,
                cancellationToken: ct);
    }
}
