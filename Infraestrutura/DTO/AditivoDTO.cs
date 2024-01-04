    using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.DTO
{
    public class AditivoDTO
    {
        public string nome { get; set; }
        public int ano { get; set; }
        public DateTime assinaturaData { get; set; }
        public string tipo { get; set; }
        public string casoAditivo { get; set; }
        public int prazo { get; set; }
        public double valorContratual { get; set; }
    }
}
