using MongoDB.Bson;

namespace CustomerApp.Infrastructure.Repositories;

internal interface IMongoEntity
{
    ObjectId _id { get; }
    string _accessId { get; set; }
}