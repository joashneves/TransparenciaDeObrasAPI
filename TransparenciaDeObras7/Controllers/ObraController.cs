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
    public class ObraController : ControllerBase
    {
        private readonly ObraContext _context;
        public ObraController(ObraContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obra>>> GetObraSet()
        {
            return await _context.Obras.ToListAsync();
        }
        [HttpGet("Pag")]
        public async Task<ActionResult<IEnumerable<Obra>>> GetPagObraSet(int pageNumber, int pageQuantity)
        {
            return await _context.Obras.Skip(pageNumber * pageQuantity).Take(pageQuantity).ToListAsync();
        }
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<Obra>>> GetPublicObraSet(int pageNumber, int pageQuantity)
        {
            var obrasPublicadas = await _context.Obras.Where(o => o.publicadoDetalhe).Skip(pageNumber * pageQuantity).Take(pageQuantity).ToListAsync();
            return Ok(obrasPublicadas);
        }
        [HttpPost]
        public IActionResult Add(Obra obra)
        {
            var obras = _context.Obras.Add(obra);
            _context.SaveChanges();
            return Ok(obras.Entity);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, Obra updatedObra)
        {
            var existingObra = _context.Obras.Find(id);

            if (existingObra == null)
            {
                return NotFound(); // Retorna 404 se a obra não for encontrada
            }

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingObra.nomeDetalhe = updatedObra.nomeDetalhe;
            existingObra.numeroDetalhe = updatedObra.numeroDetalhe;
            existingObra.situacaoDetalhe = updatedObra.situacaoDetalhe;
            existingObra.publicadoDetalhe = updatedObra.publicadoDetalhe;
            existingObra.numeroDetalhe = updatedObra.numeroDetalhe;
            existingObra.orgaoPublicoDetalhe = updatedObra.orgaoPublicoDetalhe;
            existingObra.tipoObraDetalhe = updatedObra.tipoObraDetalhe;
            existingObra.prazoInicial = updatedObra.prazoInicial;
            existingObra.prazoFinal = updatedObra.prazoFinal;
            existingObra.valorEmpenhado = updatedObra.valorEmpenhado;
            existingObra.valorLiquidado = updatedObra.valorLiquidado;
            existingObra.nomeContratadaDetalhe = updatedObra.nomeContratadaDetalhe;
            existingObra.cnpjContratadaObraDetalhe = updatedObra.cnpjContratadaObraDetalhe;
            existingObra.anoDetalhe = updatedObra.anoDetalhe;
            existingObra.licitacao = updatedObra.licitacao;
            existingObra.contrato = updatedObra.contrato;
            existingObra.localizacaoobraDetalhe = updatedObra.localizacaoobraDetalhe;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados
                return Ok(existingObra); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
    }
}
