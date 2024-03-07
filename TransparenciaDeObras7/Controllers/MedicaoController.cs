using Domain;
using Infraestrutura;
using Infraestrutura.DTO;
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
        private readonly ObraContext _contextObra;
        public MedicaoController(MedicaoContext context, ObraContext contextObra)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _contextObra = contextObra ?? throw new ArgumentException(nameof(contextObra));
        }
        [HttpGet] // Retorna os valores da Medicao
        public async Task<ActionResult<IEnumerable<Medicao>>> GetMedicaoSet()
        {
            return await _context.Medicao.ToListAsync();
        }
        [HttpPost] // Cria Medicao
        public IActionResult Add([FromForm] MedicaoViewModel medicaoViewModel)
        {
            // Obtenha o último ID da lista de Medição no banco de dados
            long ultimoId = _context.Medicao.Max(o => o.Id);

            // Incremente esse ID em 1 para obter o próximo ID disponível
            long proximoId = ultimoId + 1;

            // Defina o ID da nova Medição como o próximo ID disponível
            medicaoViewModel.Id = proximoId;

            // Verifique se o arquivo é um PDF
            if (medicaoViewModel.Medicao.ContentType.ToLower() != "application/pdf")
            {
                return BadRequest("Apenas arquivos PDF são permitidos.");
            }
            // Caminho para aonde vai o pdf
            var filePath = Path.Combine("Storage/Medicao", medicaoViewModel.Medicao.FileName);
            // Pegue o nome do caminho
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                medicaoViewModel.Medicao.CopyTo(fileStream);
            }

            var medicao = new Medicao();
            medicao.Nome = medicaoViewModel.Nome;
            medicao.ValorMedido = medicaoViewModel.ValorMedido;
            medicao.ValorPago = medicaoViewModel.ValorPago;
            medicao.Id_obras = medicaoViewModel.Id_obras;
            medicao.DataInicio = medicaoViewModel.DataInicio;
            medicao.DataFinal = medicaoViewModel.DataFinal;
            medicao.CaminhoArquivo = filePath; // Adcione no objeto medição

            var medicaoAdd = _context.Medicao.Add(medicao); // salva medição
            _context.SaveChanges();

            // Atualize o valor na obra correspondente
            AtualizarValorNaObra(medicao); 

            return Ok(medicaoAdd.Entity);
        }
        private void AtualizarValorNaObra(Medicao medicao)
        {
            // Lógica para recuperar a obra correspondente e atualizar o valor
            var obra = _contextObra.Obras.FirstOrDefault(o => o.Id == medicao.Id_obras);

            if (obra != null)
            {
                // Atualize o valor na obra com base na nova medição
                obra.ValorEmpenhado += medicao.ValorMedido;

                // Salve as alterações no banco de dados
                _contextObra.SaveChanges();
            }
        }
        [HttpGet("Download/{id}")] // Baixar arquivo PDF
        public IActionResult Download(long id)
        {
            // Obtenha o caminho do arquivo com base no ID (você precisará ajustar isso com base em como seus arquivos estão organizados)
            var medicao = _context.Medicao.Find(id);

            if (medicao == null)
            {
                return NotFound(); // Ou outra resposta adequada se o arquivo não for encontrado
            }

            var filePath = medicao.CaminhoArquivo;

            // Leia o arquivo em bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Determine o tipo MIME do arquivo
            var mimeType = "application/pdf"; // Pode precisar ajustar com base no tipo de arquivo real

            // Construa o FileContentResult para retornar o arquivo ao cliente
            var fileContentResult = new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = medicao.CaminhoArquivo // O nome do arquivo que o usuário verá ao baixar
            };

            return fileContentResult;
        }
        [HttpPut("{id}")] // Atualizar Medição
        public IActionResult Update(long id, [FromForm] MedicaoViewModel updatedMedicao)
        {
            var existingMedicao = _context.Medicao.Find(id);

            if (existingMedicao == null)
            {
                return NotFound(); // Retorna 404 se a obra não for encontrada
            }

            // Verifique se o arquivo é um PDF
            if (updatedMedicao.Medicao.ContentType.ToLower() != "application/pdf")
            {
                return BadRequest("Apenas arquivos PDF são permitidos.");
            }

            var filePath = Path.Combine("Storage/Medicao", updatedMedicao.Medicao.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                updatedMedicao.Medicao.CopyTo(fileStream);
            }

            // Salve o valor original antes da atualização
            double valorMedidoOriginal = existingMedicao.ValorMedido;

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingMedicao.Nome = updatedMedicao.Nome;
            existingMedicao.DataInicio = updatedMedicao.DataInicio;
            existingMedicao.DataFinal = updatedMedicao.DataFinal;
            existingMedicao.ValorPago = updatedMedicao.ValorPago;
            existingMedicao.ValorMedido = updatedMedicao.ValorMedido;
            existingMedicao.CaminhoArquivo = filePath;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados

                // Atualize o valor na obra correspondente
                AtualizarValorNaObra(existingMedicao, updatedMedicao.ValorMedido - valorMedidoOriginal );

                return Ok(existingMedicao); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
        private void AtualizarValorNaObra(Medicao medicao, double diferencaValorMedido)
        {
            var obra = _contextObra.Obras.FirstOrDefault(o => o.Id == medicao.Id_obras);

            if (obra != null)
            {
                // Atualize o valor na obra com base na diferença dos valores de medição
                obra.ValorEmpenhado += diferencaValorMedido;

                // Salve as alterações no banco de dados
                _contextObra.SaveChanges();
            }
            // Não é necessário retornar uma resposta específica aqui, pois o objetivo é apenas atualizar a obra.
        }

    }
}
