using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WidepollAPI.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonIgnore]
    public DateTime? CreatedAt => Id is not null ? new ObjectId(Id).CreationTime : null;
    public User Author { get; set; }
    public Statement Statement { get; set; }
    public string[] LikeIds { get; set; }
}

