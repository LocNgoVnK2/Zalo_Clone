using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class FriendList
    {
        [Column("user_1")]
        public string User1 { get; set; }
        
        [Column("user_2")]
        public string User2 { get; set; }
    }
}
