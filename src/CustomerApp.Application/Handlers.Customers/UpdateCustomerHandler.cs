using CustomerApp.Application.Common.Interfaces;
using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class UpdateCustomerHandler
{
    readonly ICustomerRepository _repository;

    public UpdateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Customer>> Handle(UpdateCustomerCommand command, CancellationToken ct)
    {
        // Get customer
        var errorOrCustomerId = CustomerId.Create(command.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        var customerId = errorOrCustomerId.Value;
        var customer = await _repository.GetCustomer(customerId, ct);

        if (customer is null) return Error.NotFound("Customer.NotFound", "The customer was not found.");

        // Update customer fields
        var errorOrCustomer = customer.Update(
            firstName: command.FirstName,
            lastName: command.LastName,
            birthdate: command.Birthdate,
            street: command.Address?.Street,
            city: command.Address?.City,
            postCode: command.Address?.PostCode,
            country: command.Address?.Country);
        if (errorOrCustomer.IsError) return errorOrCustomer.Errors;

        // Save customer
        await _repository.Update(customer, ct);

        return customer;
    }
}
