using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.ViewModel
{
    public class UserViewModel
    {
            public long Id { get; set; }
            public string Nome { get; set; }
            public string NomeCompleto { get; set; }
            public string Email { get; set; }
            public bool IsAdm { get; set; }
            public bool IsCadastrarProjeto { get; set; }
            public bool IsCadastrarAnexo { get; set; }
            public bool IsCadastrarAditivo { get; set; }
            public bool IsCadastrarFiscalGestor { get; set; }
            public bool IsCadastrarMedicao { get; set; }
            public bool IsCadastrarFoto { get; set; }
            public bool IsCadastrarOpcao { get; set; }

        
    }
}
