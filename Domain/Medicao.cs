using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("medicao")]
    public class Medicao
    {
        public long id {  get; set; } 
        public long id_obras { get; set; }
        public string nome { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFinal { get; set;  }
        public double valorPago { get; set; }
        public double valorMedido { get; set; }

    }
}
