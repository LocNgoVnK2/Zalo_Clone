using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.Data
{
    public class ZaloDbContext : DbContext
    {
        public ZaloDbContext(DbContextOptions<ZaloDbContext> options) : base(options)
        {

        }
        public DbSet<Reaction> reactions { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<BlockList> Blocks { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<FriendList> FriendLists { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageContact> MessageContacts { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }

        public DbSet<MessageToDoList> MessagesToDoLists { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }

        public DbSet<GroupChat> GroupChats { get; set; }

        public DbSet<MessageReactDetail> MessageReactDetails { get; set; }

        public DbSet<MuteGroup> MuteGroups { get; set; }

        public DbSet<MuteUser> MuteUsers { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ValidationByEmail> validationByEmails { get; set; }
        public DbSet<SignUpUser> signUpUsers { get; set; }
        public DbSet<UserContact> userContacts { get; set; }
        public DbSet<Contact> contacts {  get; set; }
        public DbSet<SearchLog> searchLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SearchLog>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).UseIdentityColumn();
            });


          modelBuilder.Entity<MuteUser>(entity =>
            {
                entity.ToTable("MUTE_USER");

                entity.HasKey(e => new { e.User, e.Receiver });

                entity.Property(e => e.User)
                    .HasColumnName("user");

                entity.Property(e => e.Receiver)
                    .HasColumnName("receiver");
            });


            modelBuilder.Entity<MuteGroup>(entity =>
            {
                entity.ToTable("MUTE_GROUP");

                entity.HasKey(e => new { e.User, e.GroupId });

                entity.Property(e => e.User)
                    .HasColumnName("user");

                entity.Property(e => e.GroupId)
                    .HasColumnName("group");
            });

            modelBuilder.Entity<ToDoUser>(entity =>
            {
                entity.ToTable("TODO_USER");

                entity.HasKey(e => new { e.TaskId, e.UserDes });

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskID");

                entity.Property(e => e.UserDes)
                    .HasColumnName("userDes");
            });

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
