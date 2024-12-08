using Web.Entities;

namespace Web.DTOs
{
    public class EventVM
    {
        public long? VisitorsCount { get; set; }
        public long EventID { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public string? Description { get; set; }
        public bool VideoFile { get; set; }
        public short? AnalisysStatus { get; set; }

        public EventVM ConvertToEventVM(Event Event, short? status)
        {
            EventVM eventVM = new()
            {
                EventID = Event.EventID,
                Name = Event.Name,
                Description = Event.Description,
                DateTime = Event.DateTime,
            };

            if (Event.VideoFileID != null)
            {
                eventVM.VideoFile = true;
                //eventVM.AnalisysStatus = status;
            }
            else
            {
                eventVM.VideoFile = false;
            }

            return eventVM;
        }
    }
}
