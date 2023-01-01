using CustomerApp.Application.Common.Interfaces;
using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class DeleteCustomerHandler
{
    readonly ICustomerRepository _repository;

    public DeleteCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(DeleteCustomerCommand command, CancellationToken ct)
    {
        var errorOrCustomerId = CustomerId.Create(command.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        var customerId = errorOrCustomerId.Value;
        await _repository.DeleteCustomer(customerId, ct);

        return new Success();
    }
}
