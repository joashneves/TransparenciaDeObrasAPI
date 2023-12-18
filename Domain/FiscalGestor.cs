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
        public long id {  get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public string papel { get; set; }
        public string secretaria { get; set; }
        public string email { get; set; }
        
    }
}
