using Curriculo4Dev.Core.Domain.Common;
using Curriculo4Dev.Core.Domain.DataTransferObjects;
using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Domain.AppService
{
    public interface IUsuarioAppService : IAppServiceBase<Usuario>
    {
        Task<MessageHelper<IEnumerable<UsuarioGetDto>>> ObterTodos(string sortKey = "PROFILE");
        Task<MessageHelper> AssinarPlano(string usuarioJson, string partitionKey, string sortKey);
    }
}
