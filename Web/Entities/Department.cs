using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("departments")]
    public class Department
    {
        [Key]
        [Column("department_id")]
        public short DepartmentID { get; set; }

        [Column("name", TypeName = "character varying(127)")]
        public string Name { get; set; } = null!;

    }
}
