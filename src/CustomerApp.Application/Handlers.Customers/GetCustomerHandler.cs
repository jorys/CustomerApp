using CustomerApp.Application.Common.Interfaces;
using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class GetCustomerHandler
{
    readonly ICustomerRepository _repository;

    public GetCustomerHandler(ICustomerRepository repository)
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
