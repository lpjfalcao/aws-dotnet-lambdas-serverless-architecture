using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Repositories;

namespace Curriculo4Dev.Core.Domain.Services
{
    public interface IPlanoService : IServiceBase<Plano>
    {

    }

    public class PlanoService(IRepositoryBase<Plano> repositoryBase) : ServiceBase<Plano>(repositoryBase), IPlanoService
    {
    }
}
