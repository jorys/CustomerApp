using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.Common.ValueObjects;

namespace CustomerApp.Application.Handlers.Authentication.Models;

public record AuthenticationResult(
    FirstName FirstName,
    LastName LastName,
    Email Email,
    string JwtToken);