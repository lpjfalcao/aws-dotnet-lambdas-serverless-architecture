using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Curriculo4Dev.Core.Domain.Repositories;

namespace Curriculo4Dev.Infra.Persistence.Repositories
{
    public class RepositoryBase<T>(IDynamoDBContext dynamoDbContext) : IRepositoryBase<T> where T : class
    {
        private IDynamoDBContext _dynamoDbContext = dynamoDbContext;

        public async Task<IEnumerable<T>> GetAll()
        {
            var scan = _dynamoDbContext.ScanAsync<T>([]);

            return await scan.GetRemainingAsync();
        }

        public async Task<IEnumerable<T>> GetAllBySortKey(string sortKey)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("SortKey", ScanOperator.Equal, sortKey)
            };

            var search = _dynamoDbContext.ScanAsync<T>(conditions);

            return await search.GetRemainingAsync();
        }

        public async Task<T> GetByPrimaryKey(string partitionKey, string sortKey)
        {
            return await _dynamoDbContext.LoadAsync<T>(partitionKey, sortKey);            
        }

        public async Task Remove(T entity)
        {
            await _dynamoDbContext.DeleteAsync(entity);
        }

        public async Task Update(T entity)
        {
            await _dynamoDbContext.SaveAsync(entity);
        }

        public async Task Create(T entity)
        {
            await _dynamoDbContext.SaveAsync(entity);
        }   
        
        public async Task<IEnumerable<T>> GetByPartitionKey(string partitionKey)
        {
            var query = _dynamoDbContext.QueryAsync<T>(partitionKey);

            return await query.GetRemainingAsync();
        }
    }
}
