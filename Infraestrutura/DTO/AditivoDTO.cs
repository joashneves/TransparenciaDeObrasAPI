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
        public string Nome { get; set; }
        public int Ano { get; set; }
        public DateTime AssinaturaData { get; set; }
        public string Tipo { get; set; }
        public string CasoAditivo { get; set; }
        public int Prazo { get; set; }
        public double ValorContratual { get; set; }
    }
}
