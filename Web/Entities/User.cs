using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long UserID { get; set; }

        [ForeignKey("employees")]
        [Column("employee_id")]
        public long EmployeeID { get; set; }

        [Column("login", TypeName = "character varying (127)")]
        public string Login { get; set; } = null!;

        [Column("password", TypeName = "character varying(255)")]
        public string Password { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;

    }
}
