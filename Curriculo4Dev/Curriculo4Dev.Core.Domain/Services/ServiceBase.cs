using Curriculo4Dev.Core.Domain.Repositories;

namespace Curriculo4Dev.Core.Domain.Services
{
    public interface IServiceBase<T> where T : class
    {
        Task Create(T entity);
        Task<T> GetByPrimaryKey(string partitionKey, string sortKey);
        Task<IEnumerable<T>> GetByPartitionKey(string partitionKey);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Remove(T entity);
    }

    public class ServiceBase<T>(IRepositoryBase<T> repositoryBase) : IServiceBase<T> where T : class
    {
        public async Task Create(T entity)
        {
            await repositoryBase.Create(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await repositoryBase.GetAll();
        }

        public async Task<T> GetByPrimaryKey(string partitionKey, string sortKey)
        {
            return await repositoryBase.GetByPrimaryKey(partitionKey, sortKey);
        }
        public async Task<IEnumerable<T>> GetByPartitionKey(string partitionKey)
        {
            return await repositoryBase.GetByPartitionKey(partitionKey);
        }

        public async Task Remove(T entity)
        {
            await repositoryBase.Remove(entity);
        }

        public async Task Update(T entity)
        {
            await repositoryBase.Update(entity);
        }
    }
}
