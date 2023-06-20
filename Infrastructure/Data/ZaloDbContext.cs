using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ZaloDbContext : IdentityDbContext<User>
    {
        public ZaloDbContext(DbContextOptions<ZaloDbContext> options) : base(options)
        {
            
        }
        public DbSet<Reaction> reactions { get; set; }

        public DbSet<UserData> userData { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<BlockList> Blocks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReceipent> MessageReceipents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable(name: "APP_USER");
                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.Id).HasColumnType("varchar");
            });
            modelBuilder.Entity<IdentityRole>(b => b.ToTable(name: "ROLE"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("USER_ROLES");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("USER_CLAIMS");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("USER_LOGIN");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("ROLE_CLAIMS");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("USER_TOKENS");
            });
        }
    }
}
