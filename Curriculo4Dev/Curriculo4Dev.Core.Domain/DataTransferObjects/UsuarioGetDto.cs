using Curriculo4Dev.Core.Application.DataTransferObjects;

namespace Curriculo4Dev.Core.Domain.DataTransferObjects
{
    public class UsuarioGetDto : BaseDto
    {
        public UsuarioGetAtributos Atributos { get; set; } = new();
    }

    public class UsuarioGetAtributos
    {
        public string Nome { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
