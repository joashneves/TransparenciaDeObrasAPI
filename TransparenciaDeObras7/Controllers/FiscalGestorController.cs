using Domain;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class FiscalGestorController : Controller
    {
        private readonly FiscalGestorContext _context;
        public FiscalGestorController(FiscalGestorContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FiscalGestor>>> GetFiscalGestorSet()
        {
            return await _context.FiscalGestors.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add(FiscalGestor fiscalGestor)
        {
            var fiscalGestors = _context.FiscalGestors.Add(fiscalGestor);
            _context.SaveChanges();
            return Ok(fiscalGestors.Entity);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, FiscalGestor updatedFiscalGestor)
        {
            var existingFiscalGestor = _context.FiscalGestors.Find(id);

            if (existingFiscalGestor == null)
            {
                return NotFound(); // Retorna 404 se a obra não for encontrada
            }

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingFiscalGestor.nome = updatedFiscalGestor.nome;
            existingFiscalGestor.papel = updatedFiscalGestor.papel;
            existingFiscalGestor.secretaria = updatedFiscalGestor.secretaria;
            existingFiscalGestor.email = updatedFiscalGestor.email;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados
                return Ok(existingFiscalGestor); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
    }
}
