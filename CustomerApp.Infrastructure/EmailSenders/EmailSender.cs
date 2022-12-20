using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.ResetPasswords.ValueObjects;
using CustomerApp.Domain.Common.ValueObjects;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CustomerApp.Infrastructure.EmailSenders;

public class EmailSender : IEmailSender
{
    readonly EmailSenderSettings _settings;
    readonly ILogger _logger;

    public EmailSender(IOptions<EmailSenderSettings> options, ILogger<EmailSender> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<bool> Send(Email email, Token token)
    {
        try
        {
            using var message = new MailMessage(from: _settings.From, to: email.Value)
            {
                Body = $"{_settings.Content}{token.Value}",
                BodyEncoding = System.Text.Encoding.UTF8,
                Subject = _settings.Subject,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            using var smtpClient = new SmtpClient(host: _settings.SmtpServer, port: _settings.Port);

            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return false;
        }
    }
}
