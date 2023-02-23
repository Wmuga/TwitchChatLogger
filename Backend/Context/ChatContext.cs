using Microsoft.EntityFrameworkCore;
using Backend.Models;


namespace Backend.Context
{
    public class ChatContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ChatContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("ChatContext"));
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Channel> Channels { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
    }
}
