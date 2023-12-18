using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public class AnexoContext : DbContext
    {
        private IConfiguration _configuration;
        public DbSet<Anexo> Anexos { get; set; } = null;
        public AnexoContext(IConfiguration configuration, DbContextOptions<AnexoContext> options) : base(options)
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
