using CustomerApp.Domain.Common;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;

public sealed class ItemQuantity : ValueObject
{
    public int Value { get; }

    ItemQuantity(int value)
    {
        Value = value;
    }

    internal static ItemQuantity Create() => new(0);

    internal ItemQuantity AddToStock(int quantity) => new(Value + quantity);

    internal ErrorOr<ItemQuantity> RemoveFromStock(int quantity)
    {
        var stockAfter = Value - quantity;
        if (stockAfter < 0)
        {
            return Errors.CannotBeNegative(nameof(ItemQuantity));
        }
        return new ItemQuantity(stockAfter);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static ItemQuantity ReloadFromRepository(int value) => new(value);
}
