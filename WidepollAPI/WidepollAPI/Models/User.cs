using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WidepollAPI.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonIgnore]
    public DateTime? CreatedAt => Id is not null ? new ObjectId(Id).CreationTime : null;
    public string Name { get; set; }
}

