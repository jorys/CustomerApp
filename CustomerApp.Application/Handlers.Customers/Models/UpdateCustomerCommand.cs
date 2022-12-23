﻿namespace CustomerApp.Application.Handlers.Customers.Models;

public record UpdateCustomerCommand(
    Guid CustomerId,
    string? FirstName,
    string? LastName,
    DateOnly? Birthdate,
    string? Email,
    UpdateCustomerCommand.AddressCommand? Address)
{
    public record AddressCommand(
        string? Street,
        string? City,
        int? PostCode,
        string? Country);
}
