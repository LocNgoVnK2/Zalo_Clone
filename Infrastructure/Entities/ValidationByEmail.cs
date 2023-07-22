using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.Entities
{
    [Table("VALIDATION_BY_EMAIL")]
    
    public class ValidationByEmail
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string ValidationCode  { get; set; }
        public DateTime ExpiredTime { get; set; }
        public bool IsActivated { get; set; }
        public int ValidationType { get; set; }


    }
}
