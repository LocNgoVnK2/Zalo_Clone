using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public enum MessageStatus{
        Sent = 0,
        Received = 1,
        Seen = 2
    }
    [Table("MESSAGE")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Sender { get; set; }
        public DateTime SendTime { get; set; }
        public int Status { get; set; }
        public int? IdMessageSrc { get; set; }
        public string? Content { get; set; }
        public bool? IsRecall { get; set; } = false;


    }
}
