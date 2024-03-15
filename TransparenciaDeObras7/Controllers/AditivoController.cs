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
        private readonly ObraContext _contextObra;
        public AditivoController(AditivoContext context, ObraContext contextObra)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _contextObra = contextObra ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aditivo>>> GetAditivoSet()
        {
            return await _context.AditivoSet.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add([FromForm] AditivoViewModel aditivoViewModel)
        {
            long proximoId;
            try
            {
                // Obtenha o último ID da lista no banco de dados
                long ultimoId = _context.AditivoSet.Any() ? _context.AditivoSet.Max(o => o.Id) : 0;

                // Incremente esse ID em 1 para obter o próximo ID disponível
                proximoId = ultimoId + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o último ID: " + ex.Message);
                return StatusCode(500, "Erro interno do servidor ao cadastrar o item.");
            }

            // Defina o ID da nova __ como o próximo ID disponível
            aditivoViewModel.Id = proximoId;

            if (aditivoViewModel.Aditivo.ContentType.ToLower() != "application/pdf")
            {
                return BadRequest("Apenas arquivos PDF são permitidos.");
            }

            var filePath = Path.Combine("Storage/Anexo", aditivoViewModel.Aditivo.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) { aditivoViewModel.Aditivo.CopyTo(fileStream);  } 
            var aditivo = new Aditivo();
            aditivo.Nome = aditivoViewModel.Nome;
            aditivo.Id_obras = aditivoViewModel.Id_obras;
            aditivo.Ano = aditivoViewModel.Ano;
            aditivo.AssinaturaData = aditivoViewModel.AssinaturaData;
            aditivo.Tipo = aditivoViewModel.Tipo;
            aditivo.CasoAditivo = aditivoViewModel.CasoAditivo;
            aditivo.Prazo = aditivoViewModel.Prazo;
            aditivo.ValorContratual = aditivoViewModel.ValorContratual;
            aditivo.CaminhoArquivo = filePath;
            var aditivoadd = _context.AditivoSet.Add(aditivo);
            _context.SaveChanges();

            AtualizarValorNaObra(aditivo);
            AtualizarValorDiasNaObra(aditivo);

            return Ok(aditivoadd.Entity);
        }
        private void AtualizarValorNaObra(Aditivo aditivo)
        {
            // Lógica para recuperar a obra correspondente e atualizar o valor
            var obra = _contextObra.Obras.FirstOrDefault(o => o.Id == aditivo.Id_obras);

            if (obra != null)
            {
                // Atualize o valor na obra com base na nova medição
                obra.ValorEmpenhado += aditivo.ValorContratual;

                // Salve as alterações no banco de dados
                _contextObra.SaveChanges();
            }
            else
            {
                // Lida com o caso em que a obra não foi encontrada (opcional)
            }
        }
        private void AtualizarValorDiasNaObra(Aditivo aditivo)
        {
            // Lógica para recuperar a obra correspondente e atualizar o valor
            var obra = _contextObra.Obras.FirstOrDefault(o => o.Id == aditivo.Id_obras);

            if (obra != null)
            {
                if (obra.PrazoInicial == 0) {
                    // Atualize o valor na obra com base na nova medição
                    obra.PrazoInicial += aditivo.Prazo;
                }else if (obra.PrazoInicial != 0)
                {
                    obra.PrazoFinal += aditivo.Prazo;
                }
                // Salve as alterações no banco de dados
                _contextObra.SaveChanges();
            }

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

            var filePath = aditivo.CaminhoArquivo;

            // Leia o arquivo em bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Determine o tipo MIME do arquivo
            var mimeType = "application/octet-stream"; // Pode precisar ajustar com base no tipo de arquivo real

            // Construa o FileContentResult para retornar o arquivo ao cliente
            var fileContentResult = new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = aditivo.CaminhoArquivo // O nome do arquivo que o usuário verá ao baixar
            };

            return fileContentResult;
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromForm] AditivoViewModel updatedAditivo)
        {
            var existingAditivo = _context.AditivoSet.Find(id);

            if (existingAditivo == null)
            {
                return NotFound(); // Retorna 404 se a Aditivo não for encontrada
            }
            if (updatedAditivo.Aditivo.ContentType.ToLower() != "application/pdf")
            {
                return BadRequest("Apenas arquivos PDF são permitidos.");
            }

            var filePath = Path.Combine("Storage/Anexo", updatedAditivo.Aditivo.FileName);

            // Salve o valor e dia original antes da atualização
            double valorContratudalOriginal = existingAditivo.ValorContratual;
            int valorPrazoOriginal = existingAditivo.Prazo;

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingAditivo.Nome = updatedAditivo.Nome;
            existingAditivo.Ano = updatedAditivo.Ano;
            existingAditivo.AssinaturaData = updatedAditivo.AssinaturaData;
            existingAditivo.Tipo = updatedAditivo.Tipo;
            existingAditivo.CasoAditivo = updatedAditivo.CasoAditivo;
            existingAditivo.Prazo = updatedAditivo.Prazo;
            existingAditivo.ValorContratual = updatedAditivo.ValorContratual;
            existingAditivo.CaminhoArquivo = filePath;

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados

                AtualizarValorNaObra(existingAditivo, updatedAditivo.ValorContratual - valorContratudalOriginal);
                AtualizarPrazoNaObra(existingAditivo, updatedAditivo.Prazo - valorPrazoOriginal);
                return Ok(existingAditivo); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
        private void AtualizarValorNaObra(Aditivo aditivo, double diferencaValorAditivo)
        {
            var obra = _contextObra.Obras.FirstOrDefault(o => o.Id == aditivo.Id_obras);

            if (obra != null)
            {
                // Atualize o valor na obra com base na diferença dos valores de medição
                obra.ValorEmpenhado += diferencaValorAditivo;

                // Salve as alterações no banco de dados
                _contextObra.SaveChanges();
            }
            // Não é necessário retornar uma resposta específica aqui, pois o objetivo é apenas atualizar a obra.
        }
        private void AtualizarPrazoNaObra(Aditivo aditivo, int diferencaPrazoAditivo)
        {
            var obra = _contextObra.Obras.FirstOrDefault(o => o.Id == aditivo.Id_obras);

            if (obra != null)
            {
                // Atualize o valor na obra com base na diferença dos valores de medição
                obra.PrazoFinal += diferencaPrazoAditivo;

                // Salve as alterações no banco de dados
                _contextObra.SaveChanges();
            }
            // Não é necessário retornar uma resposta específica aqui, pois o objetivo é apenas atualizar a obra.
        }
    }
}
