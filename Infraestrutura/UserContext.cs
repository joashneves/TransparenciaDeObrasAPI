using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infraestrutura
{
    public class UserContext : DbContext
    {
        private IConfiguration _configuration;
        public DbSet<User> Users { get; set; } = null;
        public UserContext(IConfiguration configuration, DbContextOptions<UserContext> options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ObrasData");
            optionsBuilder.UseMySQL(connectionString);
        }

    }
}
