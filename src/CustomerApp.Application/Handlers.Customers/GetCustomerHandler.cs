using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class GetCustomerHandler
{
    readonly IRepository _repository;

    public GetCustomerHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Customer>> Handle(GetCustomerQuery query, CancellationToken ct)
    {
        var errorOrCustomerId = CustomerId.Create(query.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        var customerId = errorOrCustomerId.Value;
        var customer = await _repository.GetCustomer(customerId, ct);

        if (customer is null)
        {
            return Error.NotFound(
                "CustomerId.NotFound",
                "No customer was found with this customer id.");
        }

        return customer;
    }
}
