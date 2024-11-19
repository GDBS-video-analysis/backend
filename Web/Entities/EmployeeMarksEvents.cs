using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Web.Entities
{
    [Table("employee_marks_events")]
    [Keyless]
    public class EmployeeMarksEvents
    {
        [ForeignKey("events")]
        [Column("event_id")]
        public long EventID { get; set; }

        [ForeignKey("employees")]
        [Column("employee_id")]
        public long EmployeeID { get; set; }

        [Column("videofile_mark", TypeName = "time without time zone")]
        public TimeOnly VideoFileMark { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
    }
}
