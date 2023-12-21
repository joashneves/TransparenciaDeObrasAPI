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
        public int Id { get; set; }
        public string nome { get; set; }
        public string nomeCompleto { get; set; }
        public string email { get; set; }
        public string senha_hash { get; set; }
        public bool isAdm { get; set; }
        public bool isCadastrarProjeto { get; set; }
        public bool isCadastrarAnexo { get; set; }
        public bool isCadastrarAditivo { get; set; }
         public bool isCadastrarFiscalGestor { get; set; }
        public bool isCadastrarMedicao { get; set; }
        public bool isCadastrarFoto { get; set; }
        public bool isCadastrarOpcao { get; set; }

        public void SetPassword(string password)
        {
            senha_hash = CalculateSHA256(password);
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
