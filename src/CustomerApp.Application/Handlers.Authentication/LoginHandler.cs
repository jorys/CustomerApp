using CustomerApp.Application.Handlers.Authentication.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using ErrorOr;

namespace CustomerApp.Application.Handlers.Authentication;

public sealed class LoginHandler
{
    readonly IPasswordHasher _passwordHasher;
    readonly IJwtTokenGenerator _jwtTokenGenerator;
    readonly IRepository _repository;

    public LoginHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRepository repository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _repository = repository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginCommand command)
    {
        // Get customer by email
        var errorOrEmail = Email.Create(command.Email);
        if (errorOrEmail.IsError) return errorOrEmail.Errors;
        var email = errorOrEmail.Value;

        var customer = await _repository.GetCustomer(email);
        if (customer is null) return AuthenticationError;

        // Check if account is locked
        if (customer.IsLocked) return AccountLocked;

        // Check input password
        var errorOrPassword = Password.Create(command.Password);
        if (errorOrPassword.IsError) return errorOrPassword.Errors;
        var password = errorOrPassword.Value;

        var isValidPassword = _passwordHasher.IsCorrectPassword(customer.Id, customer.HashedPassword, password);
        if (!isValidPassword) return await HandleLoginFailure(customer);

        // Save success login attempt
        var errorOrLoginAttemptSuccess = LoginAttempt.CreateSuccess(customer.Id);
        if (errorOrLoginAttemptSuccess.IsError) return AuthenticationError;
        var loginAttemptSuccess = errorOrLoginAttemptSuccess.Value;

        await _repository.Save(loginAttemptSuccess);

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

    private async Task<ErrorOr<AuthenticationResult>> HandleLoginFailure(Customer customer)
    {
        // Get last login attempt
        var lastLoginAttempt = await _repository.GetLastLoginAttempt(customer.Id);
        var errorOrLoginAttempt = lastLoginAttempt is null
            ? LoginAttempt.CreateFailed(customer.Id)
            : lastLoginAttempt.AttemptFails();

        // Too much error, lock the customer
        if (errorOrLoginAttempt.IsError && 
            errorOrLoginAttempt.FirstError.Code == Errors.MaximumLoginAttemptErrorCode)
        {
            customer.Lock();
            await _repository.Save(customer);
            return AccountLocked;
        }

        // Save login attempt
        await _repository.Save(errorOrLoginAttempt.Value);

        return AuthenticationError;
    }

    static Error AuthenticationError =>
        Error.Validation(
            "Authentication.Failed",
            "The authentication failed.");

    static Error AccountLocked =>
        Error.Validation(
            "Authentication.Locked",
            "The account is locked, please follow \"forgot your password\" process.");
}