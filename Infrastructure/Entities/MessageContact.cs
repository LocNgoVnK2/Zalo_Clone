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
    [Table("MESSAGE_CONTACT")]
    [PrimaryKey(nameof(MessageId), nameof(ContactId))]
    public class MessageContact
    {
        [Key]
        public long MessageId { get; set; }
        public string ContactId { get; set; }


    }
}
