using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WidepollAPI.Models;

public class Statement
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public User Author { get; set; }
    public string Left { get; set; }
    public string Link { get; set; }
    public string Right { get; set; }
}

