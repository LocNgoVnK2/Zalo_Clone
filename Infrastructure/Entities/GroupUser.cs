using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class GroupUser
    {
        [Key]
        [Column("idUser")]
        public string? IdUser { get; set; }
        [Key]
        [Column("idGroup")]
        public string? IdGroup { get; set; }
        [Column("idGroupRole")]
        public int? IdGroupRole { get; set; }
        [Column("joinDate")]
        public DateTime JoinDate { get; set; }
    }
}
