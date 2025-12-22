using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChromeHistoryAnalyzer.Models
{
    [Table("urls")]
    public class ChromeUrl
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("visit_count")]
        public int VisitCount { get; set; }

        [Column("typed_count")]
        public int TypedCount { get; set; }

        [Column("last_visit_time")]
        public long LastVisitTime { get; set; }

        [Column("hidden")]
        public int Hidden { get; set; }

        // Преобразование Chrome timestamp в DateTime
        [NotMapped]
        public DateTime? LastVisitDateTime
        {
            get
            {
                if (LastVisitTime == 0) return null;
                
                // Chrome хранит время как microseconds с 1 января 1601
                var epoch = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddTicks(LastVisitTime * 10);
            }
        }
    }
}