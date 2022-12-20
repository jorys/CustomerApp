﻿using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.Customers.ValueObjects;

public sealed class Birthdate : ValueObject
{
    public DateOnly Value { get; }

    Birthdate(DateOnly value)
    {
        Value = value;
    }

    public static ErrorOr<Birthdate> Create(DateOnly value)
    {
        var EighteenYearsAgo = DateOnly.FromDateTime(DateTime.UtcNow);
        if (value > EighteenYearsAgo)
        {
            return Error.Validation("Customer must have 18 years old.");
        }
        return new Birthdate(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
