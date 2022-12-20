namespace CustomerApp.Application.Handlers.Authentication.Models;

public record LoginCommand(
    string Email,
    string Password);