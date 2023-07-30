using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class GroupChat
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Column("name")]
        [NotMapped]
        public string Name { get; set; }
        [Column("image")]
        [NotMapped]
        public byte[]? Image { get; set; }
        [Column("leader")]
        public string Leader { get; set; }
    }
}
