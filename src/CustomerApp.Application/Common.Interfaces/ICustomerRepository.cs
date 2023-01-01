using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomer(Email email, CancellationToken ct);
    Task<Customer?> GetCustomer(CustomerId customerId, CancellationToken ct);

    Task Update(Customer customer, CancellationToken ct);
    Task DeleteCustomer(CustomerId customerId, CancellationToken ct);
}
