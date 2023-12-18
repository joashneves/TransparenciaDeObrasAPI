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
        public long id {  get; set; }
        public long id_obras { get; set; }
        public string nomeObra { get; set; }
        public string nome { get; set; }
        public DateTime dataHora { get; set; }
    }
}
