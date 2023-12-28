using Domain;
using Infraestrutura;
using Infraestrutura.DTO;
using Infraestrutura.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TransparenciaDeObras7.ViewModel;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AditivoController : Controller
    {
        
        private readonly AditivoContext _context;
        public AditivoController(AditivoContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aditivo>>> GetAditivoSet()
        {
            return await _context.AditivoSet.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add([FromForm] AditivoViewModel aditivoViewModel)
        {
            if (aditivoViewModel.Aditivo.ContentType.ToLower() != "application/pdf")
            {
                return BadRequest("Apenas arquivos PDF são permitidos.");
            }

            var filePath = Path.Combine("Storage/Anexo", aditivoViewModel.Aditivo.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) { aditivoViewModel.Aditivo.CopyTo(fileStream);  } 
            var aditivo = new Aditivo();
            aditivo.nome = aditivoViewModel.nome;
            aditivo.id_obras = aditivoViewModel.id_obras;
            aditivo.ano = aditivoViewModel.ano;
            aditivo.assinaturaData = aditivoViewModel.assinaturaData;
            aditivo.tipo = aditivoViewModel.tipo;
            aditivo.casoAditivo = aditivoViewModel.casoAditivo;
            aditivo.prazo = aditivoViewModel.prazo;
            aditivo.valorContratual = aditivoViewModel.valorContratual;
            aditivo.caminhoArquivo = filePath;
            var aditivoadd = _context.AditivoSet.Add(aditivo);
            _context.SaveChanges();
            return Ok(aditivoadd.Entity);
        }
        [HttpGet("Download/{id}")]
        public IActionResult Download(long id)
        {
            // Obtenha o caminho do arquivo com base no ID (você precisará ajustar isso com base em como seus arquivos estão organizados)
            var aditivo = _context.AditivoSet.Find(id);

            if (aditivo == null)
            {
                return NotFound(); // Ou outra resposta adequada se o arquivo não for encontrado
            }

            var filePath = aditivo.caminhoArquivo;

            // Leia o arquivo em bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Determine o tipo MIME do arquivo
            var mimeType = "application/octet-stream"; // Pode precisar ajustar com base no tipo de arquivo real

            // Construa o FileContentResult para retornar o arquivo ao cliente
            var fileContentResult = new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = aditivo.caminhoArquivo // O nome do arquivo que o usuário verá ao baixar
            };

            return fileContentResult;
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody] AditivoDTO updatedAditivo)
        {
            var existingAditivo = _context.AditivoSet.Find(id);

            if (existingAditivo == null)
            {
                return NotFound(); // Retorna 404 se a Aditivo não for encontrada
            }

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingAditivo.nome = updatedAditivo.nome;
            existingAditivo.ano = updatedAditivo.ano;
            existingAditivo.assinaturaData = updatedAditivo.assinaturaData;
            existingAditivo.tipo = updatedAditivo.tipo;
            existingAditivo.casoAditivo = updatedAditivo.casoAditivo;
            existingAditivo.prazo = updatedAditivo.prazo;
            existingAditivo.valorContratual = updatedAditivo.valorContratual;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados
                return Ok(existingAditivo); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
    }
}
