using Domain;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TransparenciaDeObras7.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        public UserController(UserContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUserSet()
        {
            return await _context.Users.ToListAsync();
        }
        [HttpPost]
        public IActionResult Add(User user)
        {
            var users = _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(users.Entity);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, User updatedUser)
        {
            var existingUser = _context.Users.Find(id);

            if (existingUser == null)
            {
                return NotFound(); // Retorna 404 se a user não for encontrada
            }

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
            existingUser.isCadastrarFoto = updatedUser.isCadastrarFoto;
            existingUser.isCadastrarOpcao = updatedUser.isCadastrarOpcao;

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
    }
}
