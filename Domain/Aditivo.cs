using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("aditivo")]
    public class Aditivo
    {
        public long id { get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public int ano { get; set; }
        public DateTime assinaturaData { get; set; }
        public string tipo { get; set; }
        public string casoAditivo { get; set; }
        public int prazo { get; set; }
        public double valorContratual { get; set; }
        public string caminhoArquivo { get; set; }

    }
}
