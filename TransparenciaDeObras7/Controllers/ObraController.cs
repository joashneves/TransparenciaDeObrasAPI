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
        [HttpGet] // Retorna todas as obras da API
        public async Task<ActionResult<IEnumerable<Obra>>> GetObraSet()
        {
            return await _context.Obras.ToListAsync();
        }
        [HttpGet("Pag")] // Retorna todas as obras com paginação
        public async Task<ActionResult<IEnumerable<Obra>>> GetPagObraSet(int pageNumber, int pageQuantity)
        {
            return await _context.Obras.Skip(pageNumber * pageQuantity).Take(pageQuantity).ToListAsync();
        }
        [HttpGet("public")] // Retorna todas as obras publicas
        public async Task<ActionResult<IEnumerable<Obra>>> GetPublicObraSet(int pageNumber, int pageQuantity)
        {
            var obrasPublicadas = await _context.Obras.Where(o => o.PublicadoDetalhe).Skip(pageNumber * pageQuantity).Take(pageQuantity).ToListAsync();
            return Ok(obrasPublicadas);
        }
        [HttpPost] // Cadastra a obra
        public IActionResult Add(Obra obra)
        {
            long proximoId;
            try
            {
                // Obtenha o último ID da lista de obras no banco de dados
                long ultimoId = _context.Obras.Any() ? _context.Obras.Max(o => o.Id) : 0;

                // Incremente esse ID em 1 para obter o próximo ID disponível
                proximoId = ultimoId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o último ID: " + ex.Message);
                return StatusCode(500, "Erro interno do servidor ao cadastrar a obra.");
            }

            // Defina o ID da nova obra como o próximo ID disponível
            obra.Id = proximoId;

            var obras = _context.Obras.Add(obra);
            _context.SaveChanges();
            return Ok(obras.Entity);
        }
        [HttpPost("lista")] // Cadastra uma lista da obra
        public IActionResult Add(List<Obra> obras)
        {
            long proximoId;
            try
            {
                // Obtenha o último ID da lista de obras no banco de dados
                long ultimoId = _context.Obras.Any() ? _context.Obras.Max(o => o.Id) : 0;

                // Incremente esse ID em 1 para obter o próximo ID disponível
                proximoId = ultimoId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o último ID: " + ex.Message);
                return StatusCode(500, "Erro interno do servidor ao cadastrar a obra.");
            }

            // Defina os IDs das novas obras sequencialmente
            foreach (var obra in obras)
            {
                obra.Id = proximoId++;
            }

            // Adicione as obras ao contexto
            _context.Obras.AddRange(obras);
            _context.SaveChanges();

            return Ok(obras);
        }
        [HttpPut("{id}")] // Atualiza obra
        public IActionResult Update(long id, Obra updatedObra)
        {
            var existingObra = _context.Obras.Find(id);

            if (existingObra == null)
            {
                return NotFound(); // Retorna 404 se a obra não for encontrada
            }

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingObra.NomeDetalhe = updatedObra.NomeDetalhe;
            existingObra.NumeroDetalhe = updatedObra.NumeroDetalhe;
            existingObra.SituacaoDetalhe = updatedObra.SituacaoDetalhe;
            existingObra.PublicadoDetalhe = updatedObra.PublicadoDetalhe;
            existingObra.NumeroDetalhe = updatedObra.NumeroDetalhe;
            existingObra.OrgaoPublicoDetalhe = updatedObra.OrgaoPublicoDetalhe;
            existingObra.TipoObraDetalhe = updatedObra.TipoObraDetalhe;
            existingObra.PrazoInicial = updatedObra.PrazoInicial;
            existingObra.PrazoFinal = updatedObra.PrazoFinal;
            existingObra.ValorEmpenhado = updatedObra.ValorEmpenhado;
            existingObra.ValorLiquidado = updatedObra.ValorLiquidado;
            existingObra.NomeContratadaDetalhe = updatedObra.NomeContratadaDetalhe;
            existingObra.CnpjContratadaObraDetalhe = updatedObra.CnpjContratadaObraDetalhe;
            existingObra.AnoDetalhe = updatedObra.AnoDetalhe;
            existingObra.Licitacao = updatedObra.Licitacao;
            existingObra.Contrato = updatedObra.Contrato;
            existingObra.LocalizacaoobraDetalhe = updatedObra.LocalizacaoobraDetalhe;

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
