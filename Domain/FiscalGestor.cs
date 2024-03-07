using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("fiscalgestor")]
    public class FiscalGestor
    {
        public long Id {  get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
        public string Papel { get; set; }
        public string Secretaria { get; set; }
        public string Email { get; set; }
        
    }
}
