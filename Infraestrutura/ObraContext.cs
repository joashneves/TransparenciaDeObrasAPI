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
    public class ObraContext : DbContext
    {
        private IConfiguration _configuration;
        public DbSet<Obra> Obras { get; set; } = null;

        public ObraContext(IConfiguration configuration, DbContextOptions<ObraContext> options) : base(options) 
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ObrasData");
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
