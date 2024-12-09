using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Web.Entities
{
    [Table("unregister_person_marks_events")]
    public class UnregisterPersonMarksEvents
    {
        [Key]
        [Column("unregister_person_mark_id")]
        public long UnregisterPersonMarkID { get; set; }

        [ForeignKey("events")]
        [Column("event_id")]
        public long EventID { get; set; }

        [ForeignKey("unregister_persons")]
        [Column("unregister_person_id", TypeName = "bigserial")]
        public long UnregisterPersonID { get; set; }

        [Column("videofile_mark", TypeName = "timestamp with time zone")]
        public DateTime VideoFileMark { get; set; }

        [ForeignKey("files")]
        [Column("videofile_fragment_id")]
        public long VideoFragmentID { get; set; }

        public virtual MinioFile VideoFragment { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
    }
}
