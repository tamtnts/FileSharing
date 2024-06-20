namespace DataAccess.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public bool AutoDelete { get; set; }
        public int DownloadCount { get; set; }
    }
}
