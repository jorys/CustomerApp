using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers;

public sealed class Customer : AggregateRoot<CustomerId>
{
    public override CustomerId Id { get; }
    public FirstName FirstName { get; }
    public LastName LastName { get; }
    public Birthdate Birthdate { get; }
    public Email Email { get; }
    public HashedPassword HashedPassword { get; private set; }
    public CustomerStatus Status { get; private set; }
    public Address Address { get; }

    Customer(CustomerId customerId, FirstName firstName, LastName lastName, Birthdate birthdate, Email email, HashedPassword hashedPassword, CustomerStatus status, Address address) : base(customerId)
    {
        Id = customerId;
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        Email = email;
        HashedPassword = hashedPassword;
        Status = status;
        Address = address;
    }

    public static ErrorOr<Customer> Create(string firstName, string lastName, DateOnly birthdate, string email, HashedPassword hashedPassword, ErrorOr<Address> errorOrAddress)
    {
        var errors = new List<Error>(6);

        var errorOrFirstName = FirstName.Create(firstName);
        if (errorOrFirstName.IsError) errors.AddRange(errorOrFirstName.Errors);

        var errorOrLastName = LastName.Create(lastName);
        if (errorOrLastName.IsError) errors.AddRange(errorOrLastName.Errors);

        var errorOrBirthdate = Birthdate.Create(birthdate);
        if (errorOrBirthdate.IsError) errors.AddRange(errorOrBirthdate.Errors);

        var errorOrEmail = Email.Create(email);
        if (errorOrEmail.IsError) errors.AddRange(errorOrEmail.Errors);

        var errorOrHashedPassword = HashedPassword.Create(hashedPassword.Value);
        if (errorOrHashedPassword.IsError) errors.AddRange(errorOrHashedPassword.Errors);

        if (errorOrAddress.IsError) errors.AddRange(errorOrAddress.Errors);

        if (errors.Any()) return errors;

        var customerId = CustomerId.Create();
        return new Customer(
            customerId,
            errorOrFirstName.Value,
            errorOrLastName.Value,
            errorOrBirthdate.Value,
            errorOrEmail.Value,
            errorOrHashedPassword.Value,
            CustomerStatus.CreateActive(),
            errorOrAddress.Value);
    }

    public bool IsLocked => Status.IsLocked();

    public Customer Lock()
    {
        Status = CustomerStatus.CreateLocked();
        return this;
    }

    public Customer ResetPassword(HashedPassword hashedPassword)
    {
        HashedPassword = hashedPassword;
        return this;
    }
}