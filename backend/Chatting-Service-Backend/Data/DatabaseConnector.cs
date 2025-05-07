using System;
using Chatting_Service_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Chatting_Service_Backend.Data
{
    public class DatabaseConnector : DbContext
    {
        public DatabaseConnector(DbContextOptions<DatabaseConnector> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Users table
            modelBuilder.Entity<User>()
                .Property(u => u.Chatrooms)
                .HasConversion(
                    v => string.Join(',', v), // Convert List<int> to string for storage
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                );

            // Configure the Messages table
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chatroom)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatroomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
