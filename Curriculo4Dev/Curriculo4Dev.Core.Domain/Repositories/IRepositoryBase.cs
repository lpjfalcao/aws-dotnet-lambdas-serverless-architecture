namespace Curriculo4Dev.Core.Domain.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task Create(T entity);
        Task<T> GetByPrimaryKey(string partitionKey, string sortKey);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Remove(T entity);
        Task<IEnumerable<T>> GetByPartitionKey(string partitionKey);
        Task<IEnumerable<T>> GetAllBySortKey(string sortKey);
    }
}
