using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ZaloDbContext : IdentityDbContext<UserAccount>
    {
        public ZaloDbContext(DbContextOptions<ZaloDbContext> options) : base(options)
        {
            
        }
        public DbSet<Reaction> reactions { get; set; }

        public DbSet<UserData> userData { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

    }
}
