using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("unregister_persons")]
    public class UnregisterPerson
    {
        [Key]
        [Column("unregister_person_id")]
        public long UnregisterPersonID { get; set; }

        [Column("biometrics", TypeName = "character varying(1024)")]
        public string Biometrics { get; set; } = null!;
    }
}
