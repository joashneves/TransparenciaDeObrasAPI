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
        public long id {  get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public string caminhoArquivo { get; set; }
    }
}
