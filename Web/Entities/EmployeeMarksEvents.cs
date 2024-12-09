using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web.Entities
{
    [Table("employee_marks_events")]
    public class EmployeeMarksEvents
    {
        [Key]
        [Column("employee_mark_id")]
        public long EmployeeMarkID { get; set; }

        [ForeignKey("events")]
        [Column("event_id")]
        public long EventID { get; set; }

        [ForeignKey("employees")]
        [Column("employee_id")]
        public long EmployeeID { get; set; }

        [Column("videofile_mark", TypeName = "timestamp with time zone")]
        public DateTime VideoFileMark { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
    }
}
