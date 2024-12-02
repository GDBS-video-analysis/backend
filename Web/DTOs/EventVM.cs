using Web.Entities;

namespace Web.DTOs
{
    public class EventVM
    {
        public long EventID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public VideoFileVM? VideoFile { get; set; } = null!;

        public EventVM ConvertToEventVM(Event Event)
        {
            EventVM eventVM = new()
            {
                EventID = Event.EventID,
                Name = Event.Name,
                Description = Event.Description
            };

            if (Event.VideoFile != null)
            {
                VideoFileVM? videoFileVM = new()
                {
                    FileID = Event.VideoFile.FileID,
                    Name = Event.VideoFile.Name,
                    CreatedAt = Event.VideoFile.CreatedAt,
                    MimeType = Event.VideoFile.MimeType,
                    Path = Event.VideoFile.Path,
                    Size = Event.VideoFile.Size
                };
                eventVM.VideoFile = videoFileVM;
            }

            return eventVM;
        }
    }
}
