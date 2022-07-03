namespace WidepollAPI.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid Author { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
