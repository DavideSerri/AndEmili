using AndEmili.Models;
using Microsoft.EntityFrameworkCore;

namespace AndEmili.Data
{
    public class AndEmiliContext : DbContext
    {
        private readonly IConfiguration configuration;
        public DbSet<User> Users { get; set; }
        public DbSet<UserCard> UserCards{ get; set; }


        public AndEmiliContext(DbContextOptions<AndEmiliContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("MYSQL");
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
