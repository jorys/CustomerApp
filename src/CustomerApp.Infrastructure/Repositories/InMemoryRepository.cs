using CustomerApp.Application.Interfaces;
using CustomerApp.Domain.Aggregates.Customers;
using CustomerApp.Domain.Aggregates.ResetPasswords;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.LoginAttemptAggregate;
using CustomerApp.Domain.ValueObjects;

namespace CustomerApp.Infrastructure.Repositories;

public sealed class InMemoryRepository : IRepository
{
    static readonly List<Customer> _customers = new();
    static readonly List<LoginAttempt> _loginAttempts = new();
    static readonly List<ResetPasswordResource> _resetPasswordResources = new();

    public Task DeleteCustomer(CustomerId customerId)
    {
        int index = _customers.FindIndex(savedCustomer => savedCustomer.Id == customerId);

        if (index == -1) return Task.FromResult(false);
        
        _customers.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task DeleteResetPasswordResource(CustomerId customerId)
    {
        int index = _resetPasswordResources.FindIndex(savedResource => savedResource.Id == customerId);

        if (index == -1) return Task.FromResult(false);

        _resetPasswordResources.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task<bool> DoesEmailAlreadyExist(Email email)
    {
        var doesEmailAlreadyExist = _customers.Any(customer => customer.Email == email);
        return Task.FromResult(doesEmailAlreadyExist);
    }

    public Task<Customer?> GetCustomer(CustomerId customerId)
    {
        var customer = _customers.FirstOrDefault(customer => customer.Id == customerId);
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetCustomer(Email email)
    {
        var customer = _customers.FirstOrDefault(customer => customer.Email == email);
        return Task.FromResult(customer);
    }

    public Task<LoginAttempt?> GetLastLoginAttempt(CustomerId customerId)
    {
        var lastLoginAttempt = _loginAttempts.FirstOrDefault(loginAttempt => loginAttempt.Id == customerId);
        return Task.FromResult(lastLoginAttempt);
    }

    public Task<ResetPasswordResource?> GetResetPasswordResource(Email email)
    {
        var resetPassword = _resetPasswordResources.FirstOrDefault(resetPassword => resetPassword.Email == email);
        return Task.FromResult(resetPassword);
    }

    public Task Save(Customer customer)
    {
        int index = _customers.FindIndex(savedCustomer => savedCustomer.Id == customer.Id);

        if (index != -1) _customers[index] = customer;
        else _customers.Add(customer);

        return Task.CompletedTask;
    }

    public Task Save(LoginAttempt loginAttempt)
    {
        int index = _loginAttempts.FindIndex(savedLoginAttempt => savedLoginAttempt.Id == loginAttempt.Id);
        
        if (index != -1) _loginAttempts[index] = loginAttempt;
        else _loginAttempts.Add(loginAttempt);

        return Task.CompletedTask;
    }

    public Task Save(ResetPasswordResource resetPasswordResource)
    {
        int index = _resetPasswordResources.FindIndex(savedResetPassword => savedResetPassword.Id == resetPasswordResource.Id);

        if (index != -1) _resetPasswordResources[index] = resetPasswordResource;
        else _resetPasswordResources.Add(resetPasswordResource);

        return Task.CompletedTask;
    }
}