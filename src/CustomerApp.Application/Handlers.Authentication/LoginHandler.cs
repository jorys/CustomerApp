using CustomerApp.Application.Common.Interfaces;
using CustomerApp.Application.Handlers.Authentication.Interfaces;
using CustomerApp.Application.Handlers.Authentication.Models;
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
    readonly ILoginAttemptRepository _loginRepository;
    readonly ICustomerRepository _customerRepository;

    public LoginHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILoginAttemptRepository loginRepository,
        ICustomerRepository customerRepository)
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _loginRepository = loginRepository;
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginCommand command, CancellationToken ct)
    {
        // Get customer by email
        var errorOrEmail = Email.Create(command.Email);
        if (errorOrEmail.IsError) return errorOrEmail.Errors;
        var email = errorOrEmail.Value;

        var customer = await _customerRepository.GetCustomer(email, ct);
        if (customer is null) return AuthenticationError;

        // Check if account is locked
        if (customer.IsLocked) return AccountLocked;

        // Check input password
        var errorOrPassword = Password.Create(command.Password);
        if (errorOrPassword.IsError) return errorOrPassword.Errors;
        var password = errorOrPassword.Value;

        var isValidPassword = _passwordHasher.IsCorrectPassword(customer.Id, customer.HashedPassword, password);
        if (!isValidPassword) return await HandleLoginFailure(customer, ct);

        // Save success login attempt
        var errorOrLoginAttemptSuccess = LoginAttempt.CreateSuccess(customer.Id);
        if (errorOrLoginAttemptSuccess.IsError) return AuthenticationError;
        var loginAttemptSuccess = errorOrLoginAttemptSuccess.Value;

        await _loginRepository.Upsert(loginAttemptSuccess, ct);

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

    private async Task<ErrorOr<AuthenticationResult>> HandleLoginFailure(Customer customer, CancellationToken ct)
    {
        // Get last login attempt
        var lastLoginAttempt = await _loginRepository.GetLastLoginAttempt(customer.Id, ct);
        var errorOrLoginAttempt = lastLoginAttempt is null
            ? LoginAttempt.CreateFailed(customer.Id)
            : lastLoginAttempt.AttemptFails();

        // Too much error, lock the customer
        if (errorOrLoginAttempt.IsError && 
            errorOrLoginAttempt.FirstError.Code == Errors.MaximumLoginAttemptErrorCode)
        {
            customer.Lock();
            await _customerRepository.Update(customer, ct);
            return AccountLocked;
        }

        // Save login attempt
        await _loginRepository.Upsert(errorOrLoginAttempt.Value, ct);

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