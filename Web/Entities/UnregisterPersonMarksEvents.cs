using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Web.Entities
{
    [Table("unregister_person_marks_events")]
    [Keyless]
    public class UnregisterPersonMarksEvents
    {
        [ForeignKey("events")]
        [Column("event_id")]
        public long EventID { get; set; }

        [ForeignKey("unregister_persons")]
        [Column("unregister_person_id")]
        public long UnregisterPersonID { get; set; }

        [Column("videofile_mark", TypeName = "time without time zone")]
        public TimeOnly VideoFileMark { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual UnregisterPerson UnregisterPerson { get; set; } = null!;
    }
}
