using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public long EmployeeID {  get; set; }

        [ForeignKey("posts")]
        [Column("post_id")]
        public short PostID { get; set; }

        [Column("firstname", TypeName = "character varying(127)")]
        public string FirstName { get; set; } = null!;

        [Column("lastname", TypeName = "character varying(127)")]
        public string LastName { get; set; } = null!;

        [Column("patronymic", TypeName = "character varying(127)")]
        public string? Patronymic { get; set; }

        [Column("phone_number", TypeName = "character varying(15)")]
        public string Phone { get; set; } = null!;

        //[Column("biometrics", TypeName = "character varying(1024)")]
        //public string Biometrics { get; set; } = null!;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual ICollection<MinioFile> Biometrics { get; set; } = null!;
        public virtual ICollection<Event> ExpectedEvents { get; set; } = new List<Event>();
    }
}
