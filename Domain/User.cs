using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Domain
{
    [Table("usuario")]
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Senha_hash { get; set; }
        public bool IsAdm { get; set; }
        public bool IsCadastrarProjeto { get; set; }
        public bool IsCadastrarAnexo { get; set; }
        public bool IsCadastrarAditivo { get; set; }
         public bool IsCadastrarFiscalGestor { get; set; }
        public bool IsCadastrarMedicao { get; set; }
        public bool IsCadastrarFoto { get; set; }
        public bool IsCadastrarOpcao { get; set; }

        public void SetPassword(string password)
        {
            Senha_hash = CalculateSHA256(password);
        }

        private static string CalculateSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Converte para representação hexadecimal
                }
                
                return sb.ToString();
            }
        }

    }

}
