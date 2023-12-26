using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.ViewModel
{
    public class MedicaoViewModel
    { 
        public long id { get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFinal { get; set; }
        public double valorPago { get; set; }
        public double valorMedido { get; set; }
        public IFormFile Medicao { get; set; }
    } 
}
