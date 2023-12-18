using Microsoft.AspNetCore.Http;

namespace TransparenciaDeObras7.ViewModel
{
    public class AditivoViewModel
    {
        public long id { get; set; }
        public long id_obras { get; set; }
        public string nome { get; set; }
        public int ano { get; set; }
        public DateTime assinaturaData { get; set; }
        public string tipo { get; set; }
        public string casoAditivo { get; set; }
        public int prazo { get; set; }
        public double valorContratual { get; set; }
        public IFormFile Aditivo { get; set; }

    }
}
