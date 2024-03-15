using Domain;
using Infraestrutura;
using Infraestrutura.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoContext _context;
        public HistoricoController(HistoricoContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historico>>> GetHistoricoSet(int pageNumber, int pageQuantity)
        {
            return await _context.HistoricoSet.Skip(pageNumber * pageQuantity).Take(pageQuantity).ToListAsync();
        }
        [HttpPost]
        public IActionResult Add(Historico historico)
        {
            long proximoId;
            try
            {
                // Obtenha o último ID da lista no banco de dados
                long ultimoId = _context.HistoricoSet.Any() ? _context.HistoricoSet.Max(o => o.Id) : 0;

                // Incremente esse ID em 1 para obter o próximo ID disponível
                proximoId = ultimoId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o último ID: " + ex.Message);
                return StatusCode(500, "Erro interno do servidor ao cadastrar o item.");
            }

            // Defina o ID da nova historico como o próximo ID disponível
            historico.Id = proximoId;

            var historicos = _context.HistoricoSet.Add(historico);
            _context.SaveChanges();
            return Ok(historicos.Entity);
        }
    }
}
