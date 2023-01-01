using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Application.Handlers.Customers.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomer(CustomerId customerId, CancellationToken ct);
    Task Update(Customer customer, CancellationToken ct);
    Task DeleteCustomer(CustomerId customerId, CancellationToken ct);
}
