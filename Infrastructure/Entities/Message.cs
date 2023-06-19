using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_BASE")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("userSrc")]
        public string Sender { get; set; }
        [Column("userDes")]
        public string UserDesId { get; set;}
        public DateTime BlockDate { get; set; }


    }
}
