namespace CustomerApp.Application.Handlers.Authentication.Models;

public record ResetPasswordCommand(
    string Email,
    string Token,
    string Password);