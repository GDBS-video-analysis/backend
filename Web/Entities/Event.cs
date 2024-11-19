using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("events")]
    public class Event
    {
        [Key]
        [Column("event_id")]
        public long EventID { get; set; }

        [Column("name", TypeName = "character varying(127)")]
        public string Name { get; set; } = null!;

        [Column("date_time", TypeName = "timestamp with time zone")]
        public DateTime DateTime { get; set; }

        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        [Column("videofile", TypeName = "character varying(1024)")]
        public string Videofile { get; set; } = null!;

        public virtual ICollection<Employee> ExpectedEmployees { get; set; } = new List<Employee>();
    }
}
