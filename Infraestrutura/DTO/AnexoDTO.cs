using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.DTO
{
    public class AnexoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataDocumento { get; set; }

    }
}
