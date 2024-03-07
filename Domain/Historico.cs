using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("historico")]
    public class Historico
    {
        public long Id {  get; set; }
        public long Id_obras { get; set; }
        public string NomeObra { get; set; }
        public string Nome { get; set; }
        public string NomePerfil { get; set; }
        public DateTime DataHora { get; set; }
    }
}
