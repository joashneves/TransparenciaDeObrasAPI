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
    public class HistoricoContext : DbContext
    {
        private IConfiguration _configuration;
        public DbSet<Historico> HistoricoSet { get; set; }
        public HistoricoContext(IConfiguration configuration, DbContextOptions<HistoricoContext> options) : base(options)
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
