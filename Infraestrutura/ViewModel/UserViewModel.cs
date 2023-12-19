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
            public int Id { get; set; }
            public string nome { get; set; }
            public string nomeCompleto { get; set; }
            public string email { get; set; }
            public bool isAdm { get; set; }
            public bool isCadastrarProjeto { get; set; }
            public bool isCadastrarAnexo { get; set; }
            public bool isCadastrarAditivo { get; set; }
            public bool isCadastrarFiscalGestor { get; set; }
            public bool isCadastrarMedicao { get; set; }
            public bool isCadastrarFoto { get; set; }
            public bool isCadastrarOpcao { get; set; }

        
    }
}
