using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransparenciaDeObras7.ViewModel;

namespace Infraestrutura
{
    public class FotoContext : DbContext
    {
        private IConfiguration _configuration;
        public DbSet<Foto> Foto { get; set; }
        public FotoContext(IConfiguration configuration, DbContextOptions<FotoContext> options) : base(options)
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
