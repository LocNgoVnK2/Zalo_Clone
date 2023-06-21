using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_RECEIPENT")]
    public class MessageReceipent
    {
        [Key]
        public Int64 Id { get; set; }
        public string Receiver { get; set; }


    }
}
