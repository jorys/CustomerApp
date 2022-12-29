using CustomerApp.Application.Handlers.Authentication.Models;
using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;
using CustomerApp.Domain.Common.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace CustomerApp.Application.Handlers.Authentication;

public sealed class ResetPasswordHandler
{
    readonly IRepository _repository;
    readonly IPasswordHasher _passwordHasher;
    readonly ILogger _logger;

    public ResetPasswordHandler(
        IRepository repository,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordHandler> logger)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(ResetPasswordCommand command, CancellationToken ct)
    {
        // Get reset password resource by email
        var errorOrEmail = Email.Create(command.Email);
        if (errorOrEmail.IsError) return errorOrEmail.Errors;
        var email = errorOrEmail.Value;

        var resetPasswordResource = await _repository.GetResetPasswordResource(email, ct);

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
        var customer = await _repository.GetCustomer(resetPasswordResource.Id, ct);
        if (customer is null) return GetInvalidTokenError(new { customerId = resetPasswordResource.Id });

        var errorOrCustomer = customer.UpdatePassword(hashedPassword);
        if (errorOrCustomer.IsError) return errorOrCustomer.Errors;

        // Save customer
        await _repository.Update(customer, ct);

        // Delete reset password resource
        await _repository.DeleteResetPasswordResource(resetPasswordResource.Id, ct);

        return new Success();
    }

    Error GetInvalidTokenError(object logAdditionnalProperties)
    {
        var message = "The password was not updated because of invalid token.";
        _logger.LogError(message, logAdditionnalProperties);
        return Error.Validation("ResetPassword.InvalidToken", message);
    }
}