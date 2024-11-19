using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("posts")]
    public class Post
    {
        [Key]
        [Column("post_id")]
        public short PostID { get; set; }

        [Column("name", TypeName = "character varying(127)")]
        public string Name { get; set; } = null!;

        [ForeignKey("departments")]
        [Column("department_id")]
        public short DepartmentID { get; set; }

        public virtual Department Department { get; set; } = null!;
    }
}
