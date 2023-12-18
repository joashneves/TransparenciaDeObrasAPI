using Microsoft.AspNetCore.Http;

namespace TransparenciaDeObras7.ViewModel
{
    public class FotoViewModel
    {
        public long id { get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
       // public string caminhoArquivo { get; set; }
        public IFormFile Photo { get; set; }
    }
}
