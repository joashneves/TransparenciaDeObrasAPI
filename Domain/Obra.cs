using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    // Estrutura da Obra
    [Table("obras")]
    public class Obra
    {
        public long Id { get; set; }
        public string NomeDetalhe { get; set; }
        public int NumeroDetalhe { get; set; }
        public string SituacaoDetalhe { get; set; }
        public bool PublicadoDetalhe { get; set; }
        public DateTime PublicacaoData { get; set; }
        public string OrgaoPublicoDetalhe { get; set; }
        public string TipoObraDetalhe { get; set; }
        public string NomeContratadaDetalhe { get; set; }
        public int PrazoInicial { get; set; }
        public int PrazoFinal { get; set; }
        public double ValorEmpenhado { get; set; }
        public double ValorLiquidado { get; set; }
        public string CnpjContratadaObraDetalhe { get; set; }
        public int AnoDetalhe { get; set; }
        public string Licitacao {  get; set; }
        public string Contrato { get; set; }
        public string LocalizacaoobraDetalhe { get; set; }
    }
}
