using Domain;
using Infraestrutura;
using Infraestrutura.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TransparenciaDeObras7.Services;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableRateLimiting("fixed")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        public UserController(UserContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUserSet(int pageNumber, int pageQuantity)
        {
            return await _context.Users.Skip(pageNumber * pageQuantity).Take(pageQuantity).ToListAsync();
        }
        [HttpPost]
        [DisableRateLimiting]
        [Authorize]
        public IActionResult Add(User user)
        {
            // Calcula o hash da senha antes de adicionar o usuário
            user.SetPassword(user.senha_hash);

            var users = _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(users.Entity);
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetPublicUserSet()
        {
            var users = await _context.Users
                .Select(u => new UserViewModel
                {
                Id = u.Id,
                nome = u.nome,
                nomeCompleto = u.nomeCompleto,
                email = u.email,
                isAdm = u.isAdm,
                isCadastrarProjeto = u.isCadastrarProjeto,
                isCadastrarAnexo = u.isCadastrarAnexo,
                isCadastrarAditivo = u.isCadastrarAditivo,
                isCadastrarMedicao = u.isCadastrarMedicao,
                isCadastrarFiscalGestor = u.isCadastrarFiscalGestor,
                isCadastrarFoto = u.isCadastrarFoto,
                isCadastrarOpcao = u.isCadastrarOpcao,

                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPut("{id}")]
        [DisableRateLimiting]
        [Authorize]
        public IActionResult Update(long id, User updatedUser)
        {
            var existingUser = _context.Users.Find(id);

            if (existingUser == null)
            {
                return NotFound(); // Retorna 404 se a user não for encontrada
            }

            // Calcula o hash da senha antes de adicionar o usuário
            updatedUser.SetPassword(updatedUser.senha_hash);

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingUser.nome = updatedUser.nome;
            existingUser.nomeCompleto = updatedUser.nomeCompleto;
            existingUser.email = updatedUser.email;
            existingUser.senha_hash = updatedUser.senha_hash;
            existingUser.isAdm = updatedUser.isAdm;
            existingUser.isCadastrarProjeto = updatedUser.isCadastrarProjeto;
            existingUser.isCadastrarAnexo = updatedUser.isCadastrarAnexo;
            existingUser.isCadastrarAditivo = updatedUser.isCadastrarAditivo;
            existingUser.isCadastrarMedicao = updatedUser.isCadastrarMedicao;
            existingUser.isCadastrarFiscalGestor = updatedUser.isCadastrarFiscalGestor;
            existingUser.isCadastrarFoto = updatedUser.isCadastrarFoto;
            existingUser.isCadastrarOpcao = updatedUser.isCadastrarOpcao;

            // Calcula o hash da senha antes de adicionar o usuário
            updatedUser.SetPassword(updatedUser.senha_hash);

            try
            {
                _context.SaveChanges(); // Salva as alterações no banco de dados
                return Ok(existingUser); // Retorna a obra atualizada
            }
            catch (DbUpdateException)
            {
                // Trate exceções de falha na atualização do banco de dados, se necessário
                return StatusCode(500, "Erro interno do servidor ao atualizar a obra.");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound(); // Retorna 404 se a foto não for encontrada
            }

            try
            {
                _context.Users.Remove(user); // Remova a foto do contexto do Entity Framework
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
