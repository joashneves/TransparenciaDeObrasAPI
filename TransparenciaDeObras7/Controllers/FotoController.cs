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
    public class FotoController : Controller
    {
        private readonly FotoContext _context;
        public FotoController(FotoContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Foto>>> GetFotoSet()
        {
            return await _context.Foto.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add([FromForm]FotoViewModel fotoViewModel)
        {
            // Obtenha o último ID da lista de foto no banco de dados
            long ultimoId = _context.Foto.Max(o => o.id);

            // Incremente esse ID em 1 para obter o próximo ID disponível
            long proximoId = ultimoId + 1;

            // Defina o ID da nova foto como o próximo ID disponível
            fotoViewModel.id = proximoId;

            var filePath = Path.Combine("Storage/Foto", fotoViewModel.Photo.FileName);
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            fotoViewModel.Photo.CopyTo(fileStream);
            var fotos = new Foto();
            fotos.nome = fotoViewModel.nome;
            fotos.id_obras = fotoViewModel.id_obras;
            fotos.caminhoArquivo = filePath;
            var fotoadd = _context.Foto.Add(fotos);
            _context.SaveChanges();
            return Ok(fotoadd.Entity);
        }
        [HttpGet("Download/{id}")]
        public IActionResult Download(long id)
        {
            // Obtenha o caminho do arquivo com base no ID (você precisará ajustar isso com base em como seus arquivos estão organizados)
            var foto = _context.Foto.Find(id);

            if (foto == null)
            {
                return NotFound(); // Ou outra resposta adequada se o arquivo não for encontrado
            }

            var filePath = foto.caminhoArquivo;

            // Leia o arquivo em bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Determine o tipo MIME do arquivo
            var mimeType = "application/octet-stream"; // Pode precisar ajustar com base no tipo de arquivo real

            // Construa o FileContentResult para retornar o arquivo ao cliente
            var fileContentResult = new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = foto.caminhoArquivo // O nome do arquivo que o usuário verá ao baixar
            };

            return fileContentResult;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Foto>> GetFoto(long id)
        {
            var foto = await _context.Foto.FindAsync(id);

            if (foto == null)
            {
                return NotFound(); // Retorna 404 se a foto não for encontrada
            }

            return Ok(foto); // Retorna 200 OK com a foto se for encontrada
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var foto = _context.Foto.Find(id);

            if (foto == null)
            {
                return NotFound(); // Retorna 404 se a foto não for encontrada
            }

            try
            {
                System.IO.File.Delete(foto.caminhoArquivo); // Exclua o arquivo físico do sistema de arquivos
                _context.Foto.Remove(foto); // Remova a foto do contexto do Entity Framework
                _context.SaveChanges(); // Salve as alterações no banco de dados
                return NoContent(); // Retorna 204 No Content para indicar sucesso
            }
            catch (Exception)
            {
                // Trate exceções de falha na exclusão do arquivo ou no banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao excluir a foto.");
            }
        }

    }
}
