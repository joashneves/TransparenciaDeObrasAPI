using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("anexo")]
    public class Anexo
    {
        public long Id {  get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataDocumento { get; set; }
        public string CaminhoArquivo { get; set; }
    }
}
