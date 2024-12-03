using System.ComponentModel.DataAnnotations.Schema;

namespace Web.DTOs
{
    public class EmployeeForEditVM
    {
        public long EmployeeID { get; set; }
        public short PostID { get; set; }
        public short DepartmentID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Phone { get; set; } = null!;
        public List<long>? Biometrics { get; set; }
    }
}
