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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> GetUserSet()
        {
            return await _context.Users.ToListAsync();
        }
        [HttpPost]
        [DisableRateLimiting]
        [AllowAnonymous]
        public IActionResult Add(User user)
        {
            // Obtenha o último ID da lista de users no banco de dados
            long ultimoId = _context.Users.Max(o => o.Id);

            // Incremente esse ID em 1 para obter o próximo ID disponível
            long proximoId = ultimoId + 1;

            // Defina o ID da nova user como o próximo ID disponível
            user.Id = proximoId;

            // Calcula o hash da senha antes de adicionar o usuário
            user.SetPassword(user.Senha_hash);

            // Verifica se já existe um usuário com o mesmo nome de usuário
            var existingUser = _context.Users.FirstOrDefault(u => u.Nome == user.Nome);
            if (existingUser != null)
            {
                return Conflict("Já existe um usuário com este nome de usuário.");
            }

            var users = _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(users.Entity);
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetPublicUserSet(int page, int pageSize)
        {
            var users = await _context.Users
                .Skip((page) * pageSize) // Pula os registros das páginas anteriores
                .Take(pageSize) // Define o tamanho da página
                .Select(u => new UserViewModel
                {
                Id = u.Id,
                Nome = u.Nome,
                NomeCompleto = u.NomeCompleto,
                Email = u.Email,
                IsAdm = u.IsAdm,
                IsCadastrarProjeto = u.IsCadastrarProjeto,
                IsCadastrarAnexo = u.IsCadastrarAnexo,
                IsCadastrarAditivo = u.IsCadastrarAditivo,
                IsCadastrarMedicao = u.IsCadastrarMedicao,
                IsCadastrarFiscalGestor = u.IsCadastrarFiscalGestor,
                IsCadastrarFoto = u.IsCadastrarFoto,
                IsCadastrarOpcao = u.IsCadastrarOpcao,

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
            updatedUser.SetPassword(updatedUser.Senha_hash);

            // Atualiza as propriedades da obra existente com base na obra recebida
            existingUser.Nome = updatedUser.Nome;
            existingUser.NomeCompleto = updatedUser.NomeCompleto;
            existingUser.Email = updatedUser.Email;
            existingUser.Senha_hash = updatedUser.Senha_hash;
            existingUser.IsAdm = updatedUser.IsAdm;
            existingUser.IsCadastrarProjeto = updatedUser.IsCadastrarProjeto;
            existingUser.IsCadastrarAnexo = updatedUser.IsCadastrarAnexo;
            existingUser.IsCadastrarAditivo = updatedUser.IsCadastrarAditivo;
            existingUser.IsCadastrarMedicao = updatedUser.IsCadastrarMedicao;
            existingUser.IsCadastrarFiscalGestor = updatedUser.IsCadastrarFiscalGestor;
            existingUser.IsCadastrarFoto = updatedUser.IsCadastrarFoto;
            existingUser.IsCadastrarOpcao = updatedUser.IsCadastrarOpcao;

            // Calcula o hash da senha antes de adicionar o usuário
            updatedUser.SetPassword(updatedUser.Senha_hash);

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
        [HttpPut("login")]
        [AllowAnonymous]
        public IActionResult Login(User userModel)
        {

            // Calcula o hash da senha antes de adicionar o usuário
            userModel.SetPassword(userModel.Senha_hash);

            var user = _context.Users
                .SingleOrDefault(u => u.Nome == userModel.Nome && u.Senha_hash == userModel.Senha_hash);

            if (user == null)
            {
                return BadRequest("Usuário ou senha inválidos.");
            }

            return Ok(user);
        }
    }
}
