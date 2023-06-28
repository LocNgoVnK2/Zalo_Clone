using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("TODO_USER")]
    public class ToDoUser
    {
        [Key]
        public long TaskId { get; set; }
        [Key]
        public string UserDes { get; set; }
        public int Status { get; set; } = 0;
    }
}
