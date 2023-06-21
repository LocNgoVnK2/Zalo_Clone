using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("FRIEND_REQUEST")]
    public class FriendRequest
    {
        [Key]
        [Column("user_1")]
        public string User1 { get; set; }
        [Key]
        [Column("user_2")]
        public string User2 { get; set; }
        [Column("requestDate")]
        public DateTime RequestDate { get; set; }
        [Column("acceptDate")]
        public DateTime? AcceptDate { get; set; }
    }
}
