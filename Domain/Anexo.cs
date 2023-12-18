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
        public long id {  get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public DateTime dataDocumento { get; set; }
        public string caminhoArquivo { get; set; }
    }
}
