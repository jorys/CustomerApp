using CustomerApp.Domain.Aggregates.Customers.ValueObjects;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.Common.ValueObjects;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers;

public sealed class Customer : AggregateRoot<CustomerId>
{
    public override CustomerId Id { get; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Birthdate Birthdate { get; private set; }
    public Email Email { get; private set; }
    public HashedPassword HashedPassword { get; private set; }
    public CustomerStatus Status { get; private set; }
    public Address Address { get; private set; }

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

    public static ErrorOr<Customer> Create(string firstName, string lastName, DateOnly birthdate, string email, string hashedPassword, string street, string city, int postCode, string country)
    {
        var errors = new List<Error>(9);

        var errorOrFirstName = FirstName.Create(firstName);
        if (errorOrFirstName.IsError) errors.AddRange(errorOrFirstName.Errors);

        var errorOrLastName = LastName.Create(lastName);
        if (errorOrLastName.IsError) errors.AddRange(errorOrLastName.Errors);

        var errorOrBirthdate = Birthdate.Create(birthdate);
        if (errorOrBirthdate.IsError) errors.AddRange(errorOrBirthdate.Errors);

        var errorOrEmail = Email.Create(email);
        if (errorOrEmail.IsError) errors.AddRange(errorOrEmail.Errors);

        var errorOrHashedPassword = HashedPassword.Create(hashedPassword);
        if (errorOrHashedPassword.IsError) errors.AddRange(errorOrHashedPassword.Errors);

        var errorOrAddress = Address.Create(street: street, city: city, postCode: postCode, country: country);
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

    public ErrorOr<Customer> ResetPassword(string hashedPassword)
    {
        var errorOrHashedPassword = HashedPassword.Create(hashedPassword);
        if (errorOrHashedPassword.IsError) return errorOrHashedPassword.Errors;

        HashedPassword = errorOrHashedPassword.Value;
        return this;
    }

    public ErrorOr<Customer> Update(string? firstName, string? lastName, DateOnly? birthdate, string? email, string? street, string? city, int? postCode, string? country)
    {
        var errors = new List<Error>(8);

        if (firstName is not null)
        {
            var errorOrFirstName = FirstName.Create(firstName);
            if (errorOrFirstName.IsError) errors.AddRange(errorOrFirstName.Errors);
            else FirstName = errorOrFirstName.Value;
        }
        if (lastName is not null)
        {
            var errorOrLastName = LastName.Create(lastName);
            if (errorOrLastName.IsError) errors.AddRange(errorOrLastName.Errors);
            else LastName = errorOrLastName.Value;
        }
        if (birthdate is not null)
        {
            var errorOrBirthdate = Birthdate.Create(birthdate.Value);
            if (errorOrBirthdate.IsError) errors.AddRange(errorOrBirthdate.Errors);
            else Birthdate = errorOrBirthdate.Value;
        }
        if (email is not null)
        {
            var errorOrEmail = Email.Create(email);
            if (errorOrEmail.IsError) errors.AddRange(errorOrEmail.Errors);
            else Email = errorOrEmail.Value;
        }

        var errorOrAddress = Address.With(street: street, city: city, postCode: postCode, country: country);
        if (errorOrAddress.IsError) errors.AddRange(errorOrAddress.Errors);
        else Address = errorOrAddress.Value;

        if (errors.Any()) return errors;
        else return this;
    }
}