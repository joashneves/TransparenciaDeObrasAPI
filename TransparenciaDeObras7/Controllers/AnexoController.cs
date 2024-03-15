using Domain;
using Infraestrutura;
using Infraestrutura.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TransparenciaDeObras7.ViewModel;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AnexoController : Controller
    {
        private readonly AnexoContext _context;
        public AnexoController(AnexoContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anexo>>> GetAditivoSet()
        {
            return await _context.Anexos.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add([FromForm] AnexoViewModel anexosViewModel)
        {
            long proximoId;
            try
            {
                // Obtenha o último ID da lista no banco de dados
                long ultimoId = _context.Anexos.Any() ? _context.Anexos.Max(o => o.Id) : 0;

                // Incremente esse ID em 1 para obter o próximo ID disponível
                proximoId = ultimoId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o último ID: " + ex.Message);
                return StatusCode(500, "Erro interno do servidor ao cadastrar o item.");
            }

            // Defina o ID da nova __ como o próximo ID disponível
            anexosViewModel.Id = proximoId;

            if (anexosViewModel.Anexo.ContentType.ToLower() != "application/pdf") { return BadRequest("Apenas arquivos PDF são permitidos."); }

            var filePath = Path.Combine("Storage/Anexo", anexosViewModel.Anexo.FileName);


            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) { anexosViewModel.Anexo.CopyTo(fileStream);  } 
            var anexos = new Anexo();
            anexos.Nome = anexosViewModel.Nome;
            anexos.Id_obras = anexosViewModel.Id_obras;
            anexos.Descricao = anexosViewModel.Descricao;
            anexos.DataDocumento = anexosViewModel.DataDocumento;
            anexos.CaminhoArquivo = filePath;
            var anexosadd = _context.Anexos.Add(anexos);
            _context.SaveChanges();
            return Ok(anexosadd.Entity);
        }
        [HttpGet("Download/{id}")]
        public IActionResult Download(long id)
        {
            // Obtenha o caminho do arquivo com base no ID (você precisará ajustar isso com base em como seus arquivos estão organizados)
            var anexo = _context.Anexos.Find(id);

            if (anexo == null)
            {
                return NotFound(); // Ou outra resposta adequada se o arquivo não for encontrado
            }

            var filePath = anexo.CaminhoArquivo;

            // Leia o arquivo em bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Determine o tipo MIME do arquivo
            var mimeType = "application/pdf"; // Pode precisar ajustar com base no tipo de arquivo real

            // Construa o FileContentResult para retornar o arquivo ao cliente
            var fileContentResult = new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = anexo.CaminhoArquivo // O nome do arquivo que o usuário verá ao baixar
            };

            return fileContentResult;
        }
        [HttpPut("{id}")]
        [DisableRateLimiting]
        public IActionResult Update(long id, [FromForm] AnexoViewModel updatedAnexo)
        {
            var existingAnexo = _context.Anexos.Find(id);

            if (existingAnexo == null)
            {
                return NotFound(); // Retorna 404 se a Anexo não for encontrada
            }

            if (updatedAnexo.Anexo.ContentType.ToLower() != "application/pdf") { return BadRequest("Apenas arquivos PDF são permitidos."); }

            var filePath = Path.Combine("Storage/Anexo", updatedAnexo.Anexo.FileName);

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingAnexo.Nome = updatedAnexo.Nome;
            existingAnexo.Descricao = updatedAnexo.Descricao;
            existingAnexo.DataDocumento = updatedAnexo.DataDocumento;
            existingAnexo.CaminhoArquivo = filePath;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados
                return Ok(existingAnexo); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }

    }
}
