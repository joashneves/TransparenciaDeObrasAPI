using Domain;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableRateLimiting("fixed")]
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoContext _context;
        public HistoricoController(HistoricoContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historico>>> GetHistoricoSet()
        {
            return await _context.HistoricoSet.ToListAsync();
        }
        [HttpPost]
        [DisableRateLimiting]
        public IActionResult Add(Historico historico)
        {
            var historicos = _context.HistoricoSet.Add(historico);
            _context.SaveChanges();
            return Ok(historicos.Entity);
        }
    }
}
