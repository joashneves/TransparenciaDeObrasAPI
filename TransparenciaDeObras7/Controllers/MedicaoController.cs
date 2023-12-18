using Domain;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MedicaoController : ControllerBase
    {
        private readonly MedicaoContext _context;
        public MedicaoController(MedicaoContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicao>>> GetUserSet()
        {
            return await _context.Medicao.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add(Medicao medicao)
        {
            var medicaos = _context.Medicao.Add(medicao);
            _context.SaveChanges();
            return Ok(medicaos.Entity);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, Medicao updatedMedicao)
        {
            var existingMedicao = _context.Medicao.Find(id);

            if (existingMedicao == null)
            {
                return NotFound(); // Retorna 404 se a obra não for encontrada
            }

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingMedicao.nome = updatedMedicao.nome;
            existingMedicao.dataInicio = updatedMedicao.dataInicio;
            existingMedicao.dataFinal = updatedMedicao.dataFinal;
            existingMedicao.valorPago = updatedMedicao.valorPago;
            existingMedicao.valorMedido = updatedMedicao.valorMedido;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados
                return Ok(existingMedicao); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
    }
}
