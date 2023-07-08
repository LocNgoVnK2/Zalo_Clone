using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    [Table("USER_ROLES")]
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }





    }
}
