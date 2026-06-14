using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Repositories;

namespace Curriculo4Dev.Core.Domain.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Task AssinarPlano(Usuario usuario);
        Task<IEnumerable<Usuario>> ObterTodos(string sortKey);
    }

    public class UsuarioService(IUsuarioRepository usuarioRepository) : ServiceBase<Usuario>(usuarioRepository), IUsuarioService
    {
        public async Task AssinarPlano(Usuario usuario)
        {
            usuario.Atributos?.Plano?.CalcularVigencia();

            await usuarioRepository.Create(usuario);
        }

        public async Task<IEnumerable<Usuario>> ObterTodos(string sortKey)
        {
            return await usuarioRepository.GetAllBySortKey(sortKey);
        }
    }
}
