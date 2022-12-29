using CustomerApp.Application.Handlers.Customers.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Customers;

public sealed class UpdatePasswordHandler
{
    readonly IRepository _repository;
    readonly IPasswordHasher _passwordHasher;

    public UpdatePasswordHandler(IRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<Customer>> Handle(UpdatePasswordCommand command, CancellationToken ct)
    {
        // Get customer
        var errorOrCustomerId = CustomerId.Create(command.CustomerId);
        if (errorOrCustomerId.IsError) return errorOrCustomerId.Errors;

        var customerId = errorOrCustomerId.Value;
        var customer = await _repository.GetCustomer(customerId, ct);

        if (customer is null) return Error.NotFound("Customer.NotFound", "The customer was not found.");

        // Hash password
        var errorOrPassword = Password.Create(command.Password);
        if (errorOrPassword.IsError) return errorOrPassword.Errors;
        var password = errorOrPassword.Value;

        var hashedPassword = _passwordHasher.Hash(password);

        // Update password
        var errorOrCustomer = customer.UpdatePassword(hashedPassword);
        if (errorOrCustomer.IsError) return errorOrCustomer.Errors;

        // Save customer
        await _repository.Update(customer, ct);

        return customer;
    }
}
