namespace DataAccess.Models
{
    public class Text
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string TextContent { get; set; }
        public bool AutoDelete { get; set; }
        public int AccessCount { get; set; }
    }
}
