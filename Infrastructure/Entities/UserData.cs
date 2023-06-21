using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("USER_DATA")]
    public class UserData
    {
        [Key]
        public string Id { get; set; }

        public byte[]? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public byte[]? Background { get; set; }



    }
}
