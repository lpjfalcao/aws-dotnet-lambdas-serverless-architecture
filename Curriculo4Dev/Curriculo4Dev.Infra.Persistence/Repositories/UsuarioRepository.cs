using Amazon.DynamoDBv2.DataModel;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Repositories;

namespace Curriculo4Dev.Infra.Persistence.Repositories
{
    public class UsuarioRepository(IDynamoDBContext dynamoDbContext) : RepositoryBase<Usuario>(dynamoDbContext), IUsuarioRepository
    {
    
    }
}
