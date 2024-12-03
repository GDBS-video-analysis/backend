namespace Web.DTOs
{
    public class VideoFileVM
    {
        public long FileID { get; set; }
        public string Name { get; set; } = null!;
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
