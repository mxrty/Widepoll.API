using WidepollAPI.Models;

namespace WidepollAPI.DataAccess;

public interface IDBWriter
{
    public Task<T> InsertAsync<T>(T entity) where T : DomainEntity;
    public Task<Comment> AddCommentIdToParentCommentAsync(Comment parent, string childId);
    public Task<Comment> AddLikeToCommentAsync(Comment parent, Like like);
    public Task<Post> AddLikeToPostAsync(Post parent, Like like);
}
