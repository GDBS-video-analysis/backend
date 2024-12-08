using Web.Entities;

namespace Web.DTOs
{
    public class CurrentEventVM
    {
        public long EventID { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public string? Description { get; set; }
        public VideoFileVM? VideoFile { get; set; }
        public List<EmployeeVM> ExpectedEmployees { get; set; } = [];

        public CurrentEventVM ConvertToCurrentEventVM(Event DBevent, short? status)
        {
            CurrentEventVM eventVM = new()
            {
                EventID = DBevent.EventID,
                DateTime = DBevent.DateTime,
                Description = DBevent.Description,
                Name = DBevent.Name
            };

            if (DBevent.VideoFile != null)
            {
                eventVM.VideoFile = new VideoFileVM()
                {
                    FileID = DBevent.VideoFile.FileID,
                    CreatedAt = DBevent.VideoFile.CreatedAt,
                    Name = DBevent.VideoFile.Name,
                    Size = DBevent.VideoFile.Size,
                    AnalisysStatus = status
                };
            }

            if (DBevent.ExpectedEmployees.Count != 0)
            {
                eventVM.ExpectedEmployees = DBevent.ExpectedEmployees
                    .Select(x => new EmployeeVM()
                    .ConvertToEmployeeVM(x)).ToList();
            };

            return eventVM;
        }
    }
}
