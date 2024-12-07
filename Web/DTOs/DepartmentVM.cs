namespace Web.DTOs
{
    public class DepartmentVM
    {
        public short DepartmentID { get; set; }
        public string Name { get; set; } = null!;
        public List<PostVM>? Posts { get; set; } = [];
    }
}
