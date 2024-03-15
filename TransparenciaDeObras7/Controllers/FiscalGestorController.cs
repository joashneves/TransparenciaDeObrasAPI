using Domain;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TransparenciaDeObras7.ViewModel;

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
            long proximoId;
            try
            {
                // Obtenha o último ID da lista no banco de dados
                long ultimoId = _context.FiscalGestors.Any() ? _context.FiscalGestors.Max(o => o.Id) : 0;

                // Incremente esse ID em 1 para obter o próximo ID disponível
                proximoId = ultimoId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o último ID: " + ex.Message);
                return StatusCode(500, "Erro interno do servidor ao cadastrar o item.");
            }

            // Defina o ID da nova __ como o próximo ID disponível
            fiscalGestor.Id = proximoId;
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
            existingFiscalGestor.Nome = updatedFiscalGestor.Nome;
            existingFiscalGestor.Papel = updatedFiscalGestor.Papel;
            existingFiscalGestor.Secretaria = updatedFiscalGestor.Secretaria;
            existingFiscalGestor.Email = updatedFiscalGestor.Email;

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
