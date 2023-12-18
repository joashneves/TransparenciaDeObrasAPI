using Microsoft.AspNetCore.Http;

namespace TransparenciaDeObras7.ViewModel
{
    public class AnexoViewModel
    {
        public long id { get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public DateTime dataDocumento { get; set; }
        public IFormFile Anexo { get; set; }
    }
}
