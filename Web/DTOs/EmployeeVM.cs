using Web.Entities;

namespace Web.DTOs
{
    public class EmployeeVM
    {
        public long EmployeeID { get; set; }
        public string Post { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Phone { get; set; } = null!;
        public long? AvatarID { get; set; }
        public EmployeeVM ConvertToEmployeeVM(Employee employee)
        {
            EmployeeVM employeeVM = new()
            {
                EmployeeID = employee.EmployeeID,
                Post = employee.Post.Name,
                Department = employee.Post.Department.Name,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                Patronymic = employee.Patronymic,
                AvatarID = employee.Biometrics.Count == 0 ? null : employee.Biometrics.Select(x => x.FileID)?.First(),
            };

            return employeeVM;
        }
    }
}
