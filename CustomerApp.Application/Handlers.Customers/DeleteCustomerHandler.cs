using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class DeleteCustomerHandler
{
    readonly IRepository _repository;

    public DeleteCustomerHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCustomerCommand command)
    {
        var errorOrCustomerId = CustomerId.Create(command.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        var customerId = errorOrCustomerId.Value;
        await _repository.DeleteCustomer(customerId);

        return true;
    }
}
