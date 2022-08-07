using MongoDB.Driver;
using MongoDB.Entities;
using WidepollAPI.Models;

namespace WidepollAPI.DataAccess;

public interface IDBWriter
{
    public Task<T> InsertAsync<T>(T entity) where T : DomainEntity;
    public Task<Comment> AddToParent(Comment parent, string childId);
}

public interface IDBReader
{
    public Task<T?> GetByIdAsync<T>(string id) where T : DomainEntity;
    public IReadOnlyCollection<Post> GetRecentPosts(int quantity);
}

public class MongoDB : IDBReader, IDBWriter
{
    public MongoDB()
    {

    }

    public async Task<T> InsertAsync<T>(T entity) where T : DomainEntity
    {
        await entity.SaveAsync();
        return entity;
    }

    public async Task<Comment> AddToParent(Comment parent, string childId)
    {
        return await DB.UpdateAndGet<Comment>().MatchID(parent.ID).Modify(c => c.Push(comment => comment.ReplyIds, childId)).ExecuteAsync();
    }

    public async Task<T?> GetByIdAsync<T>(string id) where T : DomainEntity
    {
        return await DB.Find<T>().OneAsync(id);
    }

    public IReadOnlyCollection<Post> GetRecentPosts(int quantity)
    {
        return DB.Queryable<Post>().OrderByDescending(p => p.CreatedOn).Take(quantity).ToList();
    }
}