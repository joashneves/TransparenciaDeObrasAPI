using Microsoft.AspNetCore.Http;

namespace TransparenciaDeObras7.ViewModel
{
    public class FotoViewModel
    {
        public long Id { get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
       // public string caminhoArquivo { get; set; }
        public IFormFile Photo { get; set; }
    }
}
