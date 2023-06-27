﻿using Infrastructure.Entities;
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

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<FriendList> FriendLists { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReceipent> MessageReceipents { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<MessageGroup> MessageGroups { get; set; }

        public DbSet<GroupRole> GroupRoles { get; set; }

        public DbSet<GroupChat> GroupChats { get; set; }

        public DbSet<MessageReactDetail> MessageReactDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.ToTable("GROUP_USER");

                entity.HasKey(e => new { e.IdUser, e.IdGroup });

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser");

                entity.Property(e => e.IdGroup)
                    .HasColumnName("idGroup");
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.ToTable("FRIEND_REQUEST"); 

                entity.HasKey(e => new { e.User1, e.User2 }); 

                entity.Property(e => e.User1)
                    .HasColumnName("user_1"); 

                entity.Property(e => e.User2)
                    .HasColumnName("user_2"); 
            });
            modelBuilder.Entity<FriendList>(entity =>
            {
                entity.ToTable("FRIEND");

                entity.HasKey(e => new { e.User1, e.User2 }); 

                entity.Property(e => e.User1)
                    .HasColumnName("user_1");

                entity.Property(e => e.User2)
                    .HasColumnName("user_2"); 
            });
        
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

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.ToTable("GROUP_ROLE");
            });
            modelBuilder.Entity<GroupChat>(entity =>
            {
                entity.ToTable("GROUP_CHAT");
            });

        }
    }
}
