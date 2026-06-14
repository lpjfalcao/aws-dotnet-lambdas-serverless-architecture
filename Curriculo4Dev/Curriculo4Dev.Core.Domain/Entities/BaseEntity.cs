using Amazon.DynamoDBv2.DataModel;

namespace Curriculo4Dev.Core.Domain.Entities
{
    [DynamoDBTable("curriculo4dev-table")]
    public class BaseEntity
    {
        [DynamoDBProperty("PK")]
        public string Id { get; set; }

        [DynamoDBProperty("SK")]
        public string SortKey { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
