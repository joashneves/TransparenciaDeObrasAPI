using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("foto")]
    public class Foto
    {
        public long Id {  get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
        public string CaminhoArquivo { get; set; }
    }
}
