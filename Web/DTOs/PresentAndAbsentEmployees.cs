namespace Web.DTOs
{
    public class PresentAndAbsentEmployees
    {
        //public List<EmployeeVM>? PresentEmployees { get; set; } = [];
        public PresentPersons PresentPersons { get; set; } = new();
        public List<EmployeeVM>? AbsentEmployees { get; set; } = [];
    }

    public class PresentPersons
    {
        public List<EmployeeVM>? ExpectedEmployees { get; set; } = [];
        public List<EmployeeVM>? NotExpectedEmployees {  get; set; } = [];
        public List<long>? UnregisterPersons { get; set; } = [];
    }
}
