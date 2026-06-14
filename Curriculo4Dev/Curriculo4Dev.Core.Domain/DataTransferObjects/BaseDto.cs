namespace Curriculo4Dev.Core.Application.DataTransferObjects
{
    public class BaseDto
    {
        public string Id { get; set; } = string.Empty;
        public string SortKey { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
