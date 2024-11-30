using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("files")]
    public class MinioFile
    {
        [Key]
        [Column("videofile_id")]
        public long VideoFileID {  get; set; }

        [Column("path", TypeName = "character varying(1024)")]
        public string Path { get; set; } = null!;

        [Column("name", TypeName = "character varying(127)")]
        public string Name { get; set; } = null!;

        [Column("size")]
        public int Size { get; set; }

        [Column("mimetype", TypeName = "character varying(127)")]
        public string MimeType { get; set; } = null!;

        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
