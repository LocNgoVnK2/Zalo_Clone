using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_TODO_LIST")]
    public class MessageToDoList
    {
        [Key]
        public long Id { get; set; }
        public long TaskId { get; set; }

    }
}
