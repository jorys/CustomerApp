using CustomerApp.Application.Handlers.Authentication.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Authentication;

public sealed class RegisterHandler
{
    readonly IPasswordHasher _passwordHasher;
    readonly IJwtTokenGenerator _jwtTokenGenerator;
    readonly IRepository _repository;

    public RegisterHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRepository repository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _repository = repository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken ct)
    {
        // Check input password
        var errorOrPassword = Password.Create(command.Password);
        if (errorOrPassword.IsError) return errorOrPassword.Errors;
        var password = errorOrPassword.Value;

        // Hash password
        var hashedPassword = _passwordHasher.Hash(password);

        // Create customer with hashed password
        var errorOrCustomer = Customer.Create(
            firstName: command.FirstName,
            lastName: command.LastName,
            birthdate: command.Birthdate,
            email: command.Email,
            hashedPassword: hashedPassword,
            street: command.Address.Street,
            city: command.Address.City,
            postCode: command.Address.PostCode,
            country: command.Address.Country
        );
        if (errorOrCustomer.IsError) return errorOrCustomer.Errors;
        var customer = errorOrCustomer.Value;

        // Check if email already exist
        var emailAlreadyExist = await _repository.DoesEmailAlreadyExist(customer.Email, ct);
        if (emailAlreadyExist)
        {
            return Error.Conflict("Email.AlreadyExist", "A customer with same email already exist.");
        }

        // Save in database
        await _repository.Insert(customer, ct);

        // Generate JWT token
        var jwtToken = _jwtTokenGenerator.GenerateToken(
            customerId: customer.Id,
            firstName: customer.FirstName,
            lastName: customer.LastName);

        return new AuthenticationResult(
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email,
            JwtToken: jwtToken);
    }
}