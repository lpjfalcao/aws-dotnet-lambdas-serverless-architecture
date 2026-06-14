using Curriculo4Dev.Core.Domain.Enums;

namespace Curriculo4Dev.Core.Domain.Entities
{
    public class Transacao : BaseEntity
    {
        public StatusTransacaoEnum Status { get; set; } = StatusTransacaoEnum.PagamentoPendente;
        public string IdCobrancaExterno { get; set; }
        public string CustomerIdExterno { get; set; }
    }
}
