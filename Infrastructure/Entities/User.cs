﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class User : IdentityUser
    {
        
        public bool IsActivated { get; set; }
        public string? RestoreMail { get; set; }

        public string? PhoneNumber { get; set; }

    }
    
}
