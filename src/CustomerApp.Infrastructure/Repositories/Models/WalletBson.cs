using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApp.Infrastructure.Repositories.Models;

internal sealed class StockItemBson
{
    [BsonElement("name")]
    internal string Name { get; init; }

    [BsonElement("quantity")]
    internal int Quantity { get; init; }

    public StockItemBson(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}
