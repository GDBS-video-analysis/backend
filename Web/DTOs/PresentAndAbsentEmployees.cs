namespace Web.DTOs
{
    public class PresentAndAbsentEmployees
    {
        public List<EmployeeVM>? PresentEmployees { get; set; } = [];
        public List<EmployeeVM>? AbsentEmployees { get; set; } = [];
    }
}
