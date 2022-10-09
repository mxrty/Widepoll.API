using MongoDB.Driver;
using MongoDB.Entities;
using WidepollAPI.Models;

namespace WidepollAPI.DataAccess;

public class MongoStore : IDBReader, IDBWriter
{
    public async Task<T> InsertAsync<T>(T entity) where T : DomainEntity
    {
        await entity.SaveAsync();
        return entity;
    }

    public async Task<Comment> AddCommentIdToParentCommentAsync(Comment parent, string childId)
    {
        return await DB.UpdateAndGet<Comment>().MatchID(parent.ID).Modify(c => c.Push(comment => comment.ReplyIds, childId)).ExecuteAsync();
    }

    public async Task<T?> GetByIdAsync<T>(string id) where T : DomainEntity
    {
        /*
         * Projection with exclusions: it is also possible to specify an exclusion projection with a new expression like so:

            var res = await DB.Find<Author>()
                  .Match(a => a.ID == "xxxxxxxxxxx")
                  .ProjectExcluding(a => new { a.Age, a.Name })
                  .ExecuteSingleAsync();
            doing so will return an Author entity with all the properties populated except for the Age and Name properties.
        */
        return await DB.Find<T>().OneAsync(id);
    }

    public IReadOnlyCollection<Post> GetRecentPosts(int quantity)
    {
        return DB.Queryable<Post>().OrderByDescending(p => p.CreatedOn).Take(quantity).ToList();
    }

    public async Task<IReadOnlyCollection<Comment>> GetCommentsByParentPostIdAsync(string id)
    {
        return await DB.Find<Comment>().ManyAsync(x => x.PostId == id);
    }

    public async Task<Comment> AddLikeToCommentAsync(Comment parent, Like like)
    {
        parent.Likes.Add(like);
        await DB.Update<Comment>().MatchID(parent.ID).ModifyWith(parent).ExecuteAsync();
        return parent;
    }

    public async Task<Post> AddLikeToPostAsync(Post parent, Like like)
    {
        parent.Likes.Add(like);
        await DB.Update<Post>().MatchID(parent.ID).ModifyWith(parent).ExecuteAsync();
        return parent;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var users = await DB.Find<User>().ManyAsync(x => x.Email == email);
        return users.SingleOrDefault();
    }
}