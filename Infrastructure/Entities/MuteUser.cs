using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class MuteUser
    {
        [Key]
        public string User { get; set; }
        [Key]
        public string Receiver { get; set; }
        public DateTime MuteTime { get; set; }
        public DateTime? EndMuteTime { get; set; }
    }
}
