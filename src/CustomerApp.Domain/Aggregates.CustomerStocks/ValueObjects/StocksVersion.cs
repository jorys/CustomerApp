using CustomerApp.Domain.Common;

namespace CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;

public sealed class StocksVersion : ValueObject
{
    public Guid Value { get; }

    StocksVersion(Guid value)
    {
        Value = value;
    }

    internal static StocksVersion CreateNew()
    {
        return new StocksVersion(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    internal static StocksVersion ReloadFromRepository(Guid value) => new(value);
}
