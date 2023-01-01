using CustomerApp.Application.Common.Interfaces;
using CustomerApp.Application.Handlers.Authentication.Interfaces;
using CustomerApp.Application.Handlers.Authentication.Models;
using CustomerApp.Application.Handlers.Authentication.Settings;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;
using CustomerApp.Domain.Common.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CustomerApp.Application.Handlers.Authentication;

public sealed class ForgotPasswordHandler
{
    readonly IResetPasswordRepository _resetPasswordRepository;
    readonly ICustomerRepository _customerRepository;
    readonly ITokenGenerator _tokenGenerator;
    readonly ResetPasswordSettings _settings;
    readonly IEmailSender _emailSender;
    readonly ILogger _logger;

    public ForgotPasswordHandler(
        IResetPasswordRepository resetPasswordRepository,
        ICustomerRepository customerRepository,
        ITokenGenerator tokenGenerator,
        IOptions<ResetPasswordSettings> options,
        IEmailSender emailSender,
        ILogger<ForgotPasswordHandler> logger)
    {
        _resetPasswordRepository = resetPasswordRepository;
        _customerRepository = customerRepository;
        _tokenGenerator = tokenGenerator;
        _settings = options.Value;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(ForgotPasswordCommand command, CancellationToken ct)
    {
        // Get customer by email
        var errorOrEmail = Email.Create(command.Email);
        if (errorOrEmail.IsError) return errorOrEmail.Errors;
        var email = errorOrEmail.Value;

        var customer = await _customerRepository.GetCustomer(email, ct);
        
        // If user does not exist, do nothing
        if (customer is null) return new Success();

        // Generate token
        var stringToken = _tokenGenerator.GenerateToken();
        var errorOrToken = Token.Create(stringToken);
        if (errorOrToken.IsError) return GetInvalidTokenError(new { customerId = customer.Id.Value, stringToken });
        var token = errorOrToken.Value;

        // Generate expiry date
        var errorOrTokenExpiry = TokenExpiry.Create(DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes));
        if (errorOrTokenExpiry.IsError) return GetInvalidTokenError(new { customerId = customer.Id.Value, _settings.ExpiryMinutes });
        var tokenExpiry = errorOrTokenExpiry.Value;

        // Create reset password resource
        var errorOrResetPassword = ResetPasswordResource.Create(customer.Id, customer.Email, token, tokenExpiry);
        if (errorOrResetPassword.IsError) return GetInvalidTokenError(new { email = customer.Email.Value, token = token.Value, tokenExpiry = tokenExpiry.Value });
        var resetPassword = errorOrResetPassword.Value;

        // Save reset password resource
        await _resetPasswordRepository.Upsert(resetPassword, ct);

        // Send email
        var success = await _emailSender.Send(email, token);
        if (!success) return Error.Unexpected("ForgotPassword.Email", "An error occurred while trying to send email.");

        return new Success();
    }

    Error GetInvalidTokenError(object logAdditionnalProperties)
    {
        var message = "A error occurred while trying to generate token";
        _logger.LogError(message, logAdditionnalProperties);
        return Error.Unexpected("ForgotPassword.InvalidToken", message);
    }
}