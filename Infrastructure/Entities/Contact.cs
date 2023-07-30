using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("CONTACT")]
    public class Contact
    {
        [Key]
        public string Id { get; set; }
        public string ContactName { get; set; }
        public byte[]? Avatar { get; set; }

    }
}
