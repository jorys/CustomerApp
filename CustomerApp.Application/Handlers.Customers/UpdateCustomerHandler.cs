using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class UpdateCustomerHandler
{
    readonly IRepository _repository;

    public UpdateCustomerHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Error>> Handle(UpdateCustomerCommand command)
    {
        // TODO
        var errorOrCustomerId = CustomerId.Create(command.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        var customerId = errorOrCustomerId.Value;
        var customer = await _repository.GetCustomer(customerId);

        if (customer is null)
        {
            return new List<Error> { Error.NotFound(
                "CustomerId.NotFound",
                "No customer was found with this customer id.") };
        }

        return new List<Error>();
    }
}
