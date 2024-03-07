using Microsoft.AspNetCore.Http;

namespace TransparenciaDeObras7.ViewModel
{
    public class AditivoViewModel
    {
        public long Id { get; set; }
        public long Id_obras { get; set; }
        public string Nome { get; set; }
        public int Ano { get; set; }
        public DateTime AssinaturaData { get; set; }
        public string Tipo { get; set; }
        public string CasoAditivo { get; set; }
        public int Prazo { get; set; }
        public double ValorContratual { get; set; }
        public IFormFile Aditivo { get; set; }

    }
}
