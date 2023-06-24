using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_GROUP")]
    public class MessageGroup
    {
        [Key]
        public long Id { get; set; }
        public string GroupReceive { get; set; }


    }
}
