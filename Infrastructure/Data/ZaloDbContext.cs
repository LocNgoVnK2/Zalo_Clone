using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ZaloDbContext : DbContext
    {
        public ZaloDbContext(DbContextOptions<ZaloDbContext> options) : base(options)
        {
            
        }
        public DbSet<Reaction> reactions { get; set; }
    }
}
