using Amazon.DynamoDBv2.DataModel;

namespace Curriculo4Dev.Core.Domain.Entities
{
    public class Template : BaseEntity
    {
        [DynamoDBProperty("Atributos")]
        public TemplateAtributos Atributos { get; set; } = new();
    }

    public class TemplateAtributos
    {
        [DynamoDBProperty("Descricao")]
        public string Descricao { get; set; } = string.Empty;

        [DynamoDBProperty("UrlImagemS3")]
        public string UrlImagemS3 { get; set; } = string.Empty;
    }
}
