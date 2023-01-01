using CustomerApp.Application.Common.Interfaces;
using CustomerApp.Application.Handlers.Authentication.Interfaces;
using CustomerApp.Application.Handlers.Authentication.Models;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;
using CustomerApp.Domain.Common.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace CustomerApp.Application.Handlers.Authentication;

public sealed class ResetPasswordHandler
{
    readonly IResetPasswordRepository _resetPasswordRepository;
    readonly ICustomerRepository _customerRepository;
    readonly IPasswordHasher _passwordHasher;
    readonly ILogger _logger;

    public ResetPasswordHandler(
        IResetPasswordRepository resetPasswordRepository,
        ICustomerRepository customerRepository,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordHandler> logger)
    {
        _resetPasswordRepository = resetPasswordRepository;
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(ResetPasswordCommand command, CancellationToken ct)
    {
        // Get reset password resource by email
        var errorOrEmail = Email.Create(command.Email);
        if (errorOrEmail.IsError) return errorOrEmail.Errors;
        var email = errorOrEmail.Value;

        var resetPasswordResource = await _resetPasswordRepository.GetResetPasswordResource(email, ct);

        // Check reset password resource exists
        if (resetPasswordResource is null) return GetInvalidTokenError(new { email.Value });

        // Check token
        var errorOrToken = Token.Create(command.Token);
        if (errorOrToken.IsError) return errorOrToken.Errors;
        var token = errorOrToken.Value;

        if (token != resetPasswordResource.Token) return GetInvalidTokenError(new { email = email.Value, token = token.Value });

        // Check token expiry
        if (resetPasswordResource.IsTokenExpired()) return Error.Validation("ResetPassword.ExpiredToken", "The token used expired, please follow the \"forgot password\" process again.");

        // Hash password
        var errorOrPassword = Password.Create(command.Password);
        if (errorOrPassword.IsError) return errorOrPassword.Errors;
        var password = errorOrPassword.Value;

        var hashedPassword = _passwordHasher.Hash(password);

        // Update customer password
        var customer = await _customerRepository.GetCustomer(resetPasswordResource.Id, ct);
        if (customer is null) return GetInvalidTokenError(new { customerId = resetPasswordResource.Id });

        var errorOrCustomer = customer.UpdatePassword(hashedPassword);
        if (errorOrCustomer.IsError) return errorOrCustomer.Errors;

        // Save customer
        await _customerRepository.Update(customer, ct);

        // Delete reset password resource
        await _resetPasswordRepository.DeleteResetPasswordResource(resetPasswordResource.Id, ct);

        return new Success();
    }

    Error GetInvalidTokenError(object logAdditionnalProperties)
    {
        var message = "The password was not updated because of invalid token.";
        _logger.LogError(message, logAdditionnalProperties);
        return Error.Validation("ResetPassword.InvalidToken", message);
    }
}