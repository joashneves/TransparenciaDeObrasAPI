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
        public string nome { get; set; }
        public string descricao { get; set; }
        public DateTime dataDocumento { get; set; }

    }
}
