namespace WidepollAPI.Ports
{
    public class StatementDto
    {
        public string Title { get; set; }
        public Guid Author { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
