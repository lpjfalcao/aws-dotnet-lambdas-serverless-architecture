using Curriculo4Dev.Core.Domain.Common;

namespace Curriculo4Dev.Core.Domain.AppService
{
    public interface IAppServiceBase<TEntity>
    {
        Task<MessageHelper> Create(string jsonRequest);
        Task<MessageHelper<TDto>> GetByPrimaryKey<TDto>(string partitionKey, string sortKey);
        Task<MessageHelper<IEnumerable<TDto>>> GetByPartitionKey<TDto>(string partitionKey);
        Task<MessageHelper<IEnumerable<Dto>>> GetAll<Dto>();
        Task<MessageHelper> Update(string partitionKey, string sortKey, string jsonRequest);
        Task<MessageHelper> Remove(string partitionKey, string sortKey);
    }
}
