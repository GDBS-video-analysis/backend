using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entities
{
    [Table("video_analisys_statuses")]
    public class VideoAnalisysStatus
    {
        [Key]
        [Column("analisys_id")]
        public long AnalisysID { get; set; }

        [ForeignKey("files")]
        [Column("videofile_id")]
        public long VideoFileID { get; set; }

        [ForeignKey("events")]
        [Column("event_id")]
        public long EventID { get; set; }

        [Column("status")]
        public short Status { get; set; } // 0 = в обработке, 1 = обработалось, 2 = ошибка

        public virtual MinioFile VideoFile { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
    }
}
