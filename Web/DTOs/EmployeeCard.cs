namespace Web.DTOs
{
    public class EmployeeCard
    {
        public bool IsPresent { get; set; }
        public EmployeeVM Employee { get; set; } = new();
        public List<DateTime> VideoMarks { get; set; } = [];
    }
}
