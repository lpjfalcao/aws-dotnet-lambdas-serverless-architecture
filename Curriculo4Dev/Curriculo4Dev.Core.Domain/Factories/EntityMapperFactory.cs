using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Mappers;

namespace Curriculo4Dev.Core.Domain.Factories
{
    public class EntityMapperFactory<TEntity> where TEntity : BaseEntity
    {
        public static IEntityMapper<TEntity> Create() 
        {
            if (typeof(TEntity) == typeof(Usuario))
                return new UsuarioMapper() as IEntityMapper<TEntity>;

            return null;
        }
    }
}
