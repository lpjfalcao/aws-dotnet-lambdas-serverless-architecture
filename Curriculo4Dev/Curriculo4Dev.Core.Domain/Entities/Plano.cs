using Curriculo4Dev.Core.Domain.Enums;

namespace Curriculo4Dev.Core.Domain.Entities
{
    public class Plano : BaseEntity
    {
        public PlanoAtributos Atributos { get; set; } = new();
    }

    public class PlanoAtributos
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public TipoPlano TipoPlano { get; set; } = new();
        public List<string> Recursos { get; set; } = [];        
    }

    public class TipoPlano
    {
        public TipoPlanoEnum Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
