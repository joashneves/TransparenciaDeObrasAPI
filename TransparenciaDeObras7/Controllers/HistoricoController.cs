using Domain;
using Infraestrutura;
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
            var historicos = _context.HistoricoSet.Add(historico);
            _context.SaveChanges();
            return Ok(historicos.Entity);
        }
    }
}
