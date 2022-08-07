using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WidepollAPI.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Body { get; set; }
    public User Author { get; set; }
    public string PostId { get; set; }
    public string ParentCommentId { get; set; }
    public string[] ReplyIds { get; set; }
    public string[] LikeIds { get; set; }
}

