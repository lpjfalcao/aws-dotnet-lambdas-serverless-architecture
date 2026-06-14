using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Domain.Mappers
{
    public interface IEntityMapper<TEntity> where TEntity : BaseEntity
    {
        TEntity MapFrom(string json, TEntity entity);
    }
}
