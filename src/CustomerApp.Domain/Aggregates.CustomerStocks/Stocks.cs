using CustomerApp.Domain.Aggregates.CustomerStocks.ValueObjects;
using CustomerApp.Domain.Common;
using CustomerApp.Domain.ValueObjects;
using ErrorOr;

namespace CustomerApp.Domain.Aggregates.CustomerStocks;

public sealed class Stocks : AggregateRoot<CustomerId>
{
    public override CustomerId Id { get; }
    public StocksVersion Version { get; private set; }
    readonly Dictionary<ItemName, ItemQuantity> _items; 
    public IReadOnlyDictionary<ItemName, ItemQuantity> Items { get => _items; }

    Stocks(CustomerId customerId, StocksVersion version, Dictionary<ItemName, ItemQuantity> items) : base(customerId)
    {
        Id = customerId;
        Version = version;
        _items = items;
    }

    public static Stocks Create(CustomerId customerId)
    {
        return new Stocks(
            customerId: customerId,
            version: StocksVersion.CreateNew(),
            items: new Dictionary<ItemName, ItemQuantity>());
    }

    public ErrorOr<Stocks> AddToStock(string itemName, int quantityToAdd)
    {
        if (quantityToAdd < 1) return Errors.MustBePositive("Quantity");

        var errorOrItemName = ItemName.Create(itemName);
        if (errorOrItemName.IsError) return errorOrItemName.Errors;
        var name = errorOrItemName.Value;

        var previousQuantity = _items.GetValueOrDefault(name)
            ?? ItemQuantity.Create();

        var newQuantity = previousQuantity.AddToStock(quantityToAdd);

        _items[name] = newQuantity;
        Version = StocksVersion.CreateNew();

        return this;
    }

    public ErrorOr<Stocks> RemoveFromStock(string itemName, int quantityToRemove)
    {
        if (quantityToRemove < 1) return Errors.MustBePositive("Quantity");

        var errorOrItemName = ItemName.Create(itemName);
        if (errorOrItemName.IsError) return errorOrItemName.Errors;
        var name = errorOrItemName.Value;

        var previousQuantity = _items.GetValueOrDefault(name)
            ?? ItemQuantity.Create();

        var errorOrNewQuantity = previousQuantity.RemoveFromStock(quantityToRemove);
        if (errorOrNewQuantity.IsError) return errorOrNewQuantity.Errors;

        _items[name] = errorOrNewQuantity.Value;
        Version = StocksVersion.CreateNew();

        return this;
    }

    public static Stocks ReloadFromRepository(Guid customerId, Guid stocksVersion, Dictionary<string, int> stockItems) =>
        new(
            customerId: CustomerId.ReloadFromRepository(customerId),
            version: StocksVersion.ReloadFromRepository(stocksVersion),
            items: stockItems.ToDictionary(
                item => ItemName.ReloadFromRepository(item.Key),
                item => ItemQuantity.ReloadFromRepository(item.Value)));
}
