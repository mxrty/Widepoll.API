using WidepollAPI.Models;

namespace WidepollAPI
{
    public interface ILikeRepository
    {
        public bool TryGet(Guid id, out Like like);
        public void Save(Like like);
    }

    public class LikeRepository : ILikeRepository
    {
        public Dictionary<Guid, Like> Likes { get; set; } 

        public LikeRepository()
        {
            Likes = new Dictionary<Guid, Like>();  
        }

        public void Save(Like like)
        {
            Likes.Add(like.Id, like);
        }

        public bool TryGet(Guid id, out Like like)
        {
            if (Likes.TryGetValue(id, out like))
            {
                return true;
            }
            return false;
        }
    }
}
