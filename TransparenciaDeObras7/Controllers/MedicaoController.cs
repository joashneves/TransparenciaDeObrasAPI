using Domain;
using Infraestrutura;
using Infraestrutura.ViewModel;
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
        public async Task<ActionResult<IEnumerable<Medicao>>> GetMedicaoSet()
        {
            return await _context.Medicao.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add([FromForm] MedicaoViewModel medicaoViewModel)
        {
            // Verifique se o arquivo é um PDF
            if (medicaoViewModel.Medicao.ContentType.ToLower() != "application/pdf")
            {
                return BadRequest("Apenas arquivos PDF são permitidos.");
            }

            var filePath = Path.Combine("Storage/Medicao", medicaoViewModel.Medicao.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                medicaoViewModel.Medicao.CopyTo(fileStream);
            }

            var medicao = new Medicao();
            medicao.nome = medicaoViewModel.nome;
            medicao.valorMedido = medicaoViewModel.valorMedido;
            medicao.valorPago = medicaoViewModel.valorPago;
            medicao.id_obras = medicaoViewModel.id_obras;
            medicao.dataInicio = medicaoViewModel.dataInicio;
            medicao.dataFinal = medicaoViewModel.dataFinal;
            medicao.caminhoArquivo = filePath;

            var medicaoAdd = _context.Medicao.Add(medicao);
            _context.SaveChanges();

            return Ok(medicaoAdd.Entity);
        }
        [HttpGet("Download/{id}")]
        public IActionResult Download(long id)
        {
            // Obtenha o caminho do arquivo com base no ID (você precisará ajustar isso com base em como seus arquivos estão organizados)
            var medicao = _context.Medicao.Find(id);

            if (medicao == null)
            {
                return NotFound(); // Ou outra resposta adequada se o arquivo não for encontrado
            }

            var filePath = medicao.caminhoArquivo;

            // Leia o arquivo em bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Determine o tipo MIME do arquivo
            var mimeType = "application/pdf"; // Pode precisar ajustar com base no tipo de arquivo real

            // Construa o FileContentResult para retornar o arquivo ao cliente
            var fileContentResult = new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = medicao.caminhoArquivo // O nome do arquivo que o usuário verá ao baixar
            };

            return fileContentResult;
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
