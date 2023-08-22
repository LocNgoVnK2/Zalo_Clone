using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_GROUP")]
    [PrimaryKey(nameof(MessageId), nameof(GroupId))]
    public class MessageGroup
    {
        [Key]
        public long MessageId { get; set; }
        public string GroupId { get; set; }
        public bool IsCompleted {get ; set;}


    }
}
