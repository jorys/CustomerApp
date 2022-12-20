namespace CustomerApp.Infrastructure.EmailSenders;

public sealed class EmailSenderSettings
{
    public string SmtpServer { get; init; } = "";
    public int Port { get; init; }
    public string From { get; init; } = "";
    public string Content { get; init; } = "";
    public string Subject { get; init; } = "";
}