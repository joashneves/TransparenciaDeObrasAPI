using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("obras")]
    public class Obra
    {
        public long id { get; set; }
        public string nomeDetalhe { get; set; }
        public int numeroDetalhe { get; set; }
        public string situacaoDetalhe { get; set; }
        public bool publicadoDetalhe { get; set; }
        public DateTime publicacaoData { get; set; }
        public string orgaoPublicoDetalhe { get; set; }
        public string tipoObraDetalhe { get; set; }
        public double valorPagoDetalhe { get; set; }
        public string nomeContratadaDetalhe { get; set; }
        public string cnpjContratadaObraDetalhe { get; set; }
        public int anoDetalhe { get; set; }
        public string licitacao {  get; set; }
        public string contrato { get; set; }
        public string localizacaoobraDetalhe { get; set; }
    }
}
