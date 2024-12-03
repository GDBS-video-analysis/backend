namespace Web.DTOs
{
    public class EmployeeVM
    {
        public long EmployeeID { get; set; }
        public string Post { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Phone { get; set; } = null!;
        public long? Avatar { get; set; }
    }
}
