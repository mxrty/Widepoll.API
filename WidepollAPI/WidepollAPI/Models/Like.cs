using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WidepollAPI.Models;

public class Like
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonIgnore]
    public DateTime? CreatedAt => Id is not null ? new ObjectId(Id).CreationTime : null;
    public string AuthorId { get; set; }
    public string PostId { get; set; }
    public string CommentId { get; set; }
}

