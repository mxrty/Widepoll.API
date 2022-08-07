using MongoDB.Driver;
using WidepollAPI.Models;

namespace WidepollAPI.DataAccess;

public interface IDBWriter
{
    public Task<User> InsertAsync(User user);
    public Task<Statement> InsertAsync(Statement statement);
    public Task<Post> InsertAsync(Post post);
    public Task<Comment> InsertAsync(Comment comment);
    public Task<Like> InsertAsync(Like like);
    public Task AddToParent(Comment parent, string childId);
}

public interface IDBReader
{
    public Task<User?> GetUser(string id);
    public Task<Post?> GetPost(string id);
    public Task<IReadOnlyCollection<Post>> GetRecentPosts(int quantity);
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

    public async Task<IReadOnlyCollection<Post>> GetRecentPosts(int quantity)
    {
        var result = posts.AsQueryable()
            .OrderByDescending(p => p.CreatedAt.Value)
            .Take(quantity);

        return result.ToList();
    }

    public async Task<User?> GetUser(string id)
    {
        var result = await users.FindAsync(u => u.Id == id);
        return result.SingleOrDefault();
    }

    public async Task<User> InsertAsync(User user)
    {
        await users.InsertOneAsync(user);
        return user;
    }

    public async Task<Statement> InsertAsync(Statement statement)
    {
        await statements.InsertOneAsync(statement);
        return statement;
    }

    public async Task<Post> InsertAsync(Post post)
    {
        await posts.InsertOneAsync(post);
        return post;
    }

    public async Task<Comment> InsertAsync(Comment comment)
    {
        await comments.InsertOneAsync(comment);
        return comment;
    }

    public async Task<Like> InsertAsync(Like like)
    {
        await likes.InsertOneAsync(like);
        return like;
    }
}

