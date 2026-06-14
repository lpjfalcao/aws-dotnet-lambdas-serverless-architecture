using Amazon.DynamoDBv2.DataModel;

namespace Curriculo4Dev.Core.Domain.Entities
{
    public class Documento : BaseEntity
    {
        [DynamoDBProperty("Atributos")]
        public DocumentoAtributos Atributos { get; set; } = new();
    }

    public class DocumentoAtributos
    {
        public string ConfiguracaoJson { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
    }
}

