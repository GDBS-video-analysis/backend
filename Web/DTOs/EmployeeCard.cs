namespace Web.DTOs
{
    public class EmployeeCard
    {
        public bool IsPresent { get; set; }
        public EmployeeVM Employee { get; set; } = new();
        public List<TimeOnly> VideoMarks { get; set; } = [];
    }
}
