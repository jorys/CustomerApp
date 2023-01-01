using CustomerApp.Application.Handlers.Authentication.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Infrastructure.Repositories.Models;
using MongoDB.Driver;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class RegisterMongoRepository : IRegisterRepository
{
    readonly IMongoCollection<CustomerBson> _customersCollection;

    public RegisterMongoRepository(IMongoClient mongoClient)
    {
        var customerDb = mongoClient.GetDatabase(MongoDatabaseNames.CustomerDb);

        _customersCollection = customerDb.GetCollection<CustomerBson>(MongoDatabaseNames.CustomersCollectionName);
    }

    public Task<bool> DoesEmailAlreadyExist(Email email, CancellationToken ct)
    {
        return _customersCollection
            .Find(customer => customer.Email == email.Value)
            .AnyAsync(ct);
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
}
