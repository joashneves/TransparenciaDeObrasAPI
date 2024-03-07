using Microsoft.AspNetCore.Http;

namespace TransparenciaDeObras7.ViewModel
{
    public class AnexoViewModel
    {
        public long Id { get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataDocumento { get; set; }
        public IFormFile Anexo { get; set; }
    }
}
