using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;

public sealed class ItemName : ValueObject
{
    public string Value { get; }

    ItemName(string value)
    {
        Value = value;
    }

    internal static ErrorOr<ItemName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.IsRequired(nameof(ItemName));
        }
        return new ItemName(value.ToLowerInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static ItemName ReloadFromRepository(string value) => new(value);
}
