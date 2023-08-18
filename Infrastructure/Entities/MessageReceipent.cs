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
    [Table("MESSAGE_RECEIPENT")]
    [PrimaryKey(nameof(MessageId), nameof(UserId))]
    public class MessageReceipent
    {
        [Key]
        public long MessageId { get; set; }
        public string UserId { get; set; }
        public string? GroupId { get; set; }
        public MessageStatus Status {get ; set;}


    }
}
