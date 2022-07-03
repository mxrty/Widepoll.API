using WidepollAPI.Ports;

namespace WidepollAPI
{
    public class Statement
    {
        public Guid Id { get; set; }
        public Guid Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid[] LikeIds { get; set; }
        public Guid[] CommentIds { get; set; }
        public DateTimeOffset CreatedAt { get; set; }


        public Statement()
        {
            Id = Guid.NewGuid();
        }

        public void Restore(StatementDto dto)
        {
            Author = dto.Author;
            Title = dto.Title;
            Description = dto.Description;
            CreatedAt = dto.CreatedAt;
        }
    }
}