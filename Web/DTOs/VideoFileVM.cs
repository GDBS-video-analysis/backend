namespace Web.DTOs
{
    public class VideoFileVM
    {
        public long FileID { get; set; }
        public string Path { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long Size { get; set; }
        public string MimeType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
