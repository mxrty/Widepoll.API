using MongoDB.Driver;
using WidepollAPI.Models;

namespace WidepollAPI.DataAccess;

public interface IDBWriter
{
    public Task Insert(User user);
    public Task Insert(Statement statement);
    public Task Insert(Post post);
    public Task Insert(Comment comment);
    public Task Insert(Like like);
    public Task AddToParent(Comment parent, string childId);
}

public interface IDBReader
{
    public Task<User?> GetUser(string id);
    public Task<Post?> GetPost(string id);
    public Task<Comment?> GetComment(string id);
    public Task<Like?> GetLike(string id);
}

public class MongoDB : IDBReader, IDBWriter
{
    private IMongoDatabase db;
    private IMongoCollection<User> users;
    private IMongoCollection<Statement> statements;
    private IMongoCollection<Post> posts;
    private IMongoCollection<Comment> comments;
    private IMongoCollection<Like> likes;
    private string password = "deUsKxSO8O2zBxSr";

    public MongoDB()
    {
        var settings = MongoClientSettings.FromConnectionString($"mongodb+srv://admin:{password}@cluster0.j8fil.mongodb.net/?retryWrites=true&w=majority");
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);
        db = client.GetDatabase("test");
        users = db.GetCollection<User>("users");
        statements = db.GetCollection<Statement>("statements");
        posts = db.GetCollection<Post>("posts");
        comments = db.GetCollection<Comment>("comments");
        likes = db.GetCollection<Like>("likes");
    }

    public async Task AddToParent(Comment parent, string childId)
    {
        var filter = Builders<Comment>.Filter.Eq("Id", parent.Id);
        var update = Builders<Comment>.Update.AddToSet(c => c.ReplyIds, childId);
        await comments.UpdateOneAsync(filter, update);
    }

    public async Task<Comment?> GetComment(string id)
    {
        var result = await comments.FindAsync(s => s.Id == id);
        return result.SingleOrDefault();
    }

    public async Task<Like?> GetLike(string id)
    {
        var result = await likes.FindAsync(s => s.Id == id);
        return result.SingleOrDefault();
    }

    public async Task<Post?> GetPost(string id)
    {
        var result = await posts.FindAsync(s => s.Id == id);
        return result.SingleOrDefault();
    }

    public async Task<User?> GetUser(string id)
    {
        var result = await users.FindAsync(u => u.Id == id);
        return result.SingleOrDefault();
    }

    public async Task Insert(User user)
    {
        await users.InsertOneAsync(user);
    }

    public async Task Insert(Statement statement)
    {
        await statements.InsertOneAsync(statement);
    }

    public async Task Insert(Post post)
    {
        await posts.InsertOneAsync(post);
    }

    public async Task Insert(Comment comment)
    {
        await comments.InsertOneAsync(comment);
    }

    public async Task Insert(Like like)
    {
        await likes.InsertOneAsync(like);
    }
}

