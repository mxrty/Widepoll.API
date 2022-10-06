using WidepollAPI.Models;

namespace WidepollAPI.DataAccess;

public interface IDBReader
{
    public Task<T?> GetByIdAsync<T>(string id) where T : DomainEntity;
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<IReadOnlyCollection<Comment>> GetCommentsByParentPostIdAsync(string id);
    public IReadOnlyCollection<Post> GetRecentPosts(int quantity);
}
