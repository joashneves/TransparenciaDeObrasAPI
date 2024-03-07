using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.DTO
{
    public class MedicaoDTO
    {
        public long Id { get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public double ValorPago { get; set; }
        public double ValorMedido { get; set; }
    }
}
