using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Enums;
using System.Text.Json;

namespace Curriculo4Dev.Core.Domain.Mappers
{
    public class UsuarioMapper : IEntityMapper<Usuario>
    {
        private const int PLANO_BASICO = 1;

        public Usuario MapFrom(string json, Usuario usuario)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("Atributos", out var atributos))
            {
                usuario.Atributos ??= new UsuarioAtributos();

                if (atributos.TryGetProperty("Nome", out var nome))
                {
                    usuario.Atributos.Nome = nome.GetString()!;
                }

                if (atributos.TryGetProperty("Email", out var email))
                {
                    usuario.Atributos.Email = email.GetString()!;
                }

                if (atributos.TryGetProperty("Plano", out var plano))
                {
                    usuario.Atributos.Plano = new();

                    if (plano.TryGetProperty("TipoPlano", out var tipoPlano))
                    {
                        var tipo = tipoPlano.GetInt32();
                        usuario.Atributos.Plano.TipoPlano = tipo == PLANO_BASICO ? TipoPlanoEnum.Basico : TipoPlanoEnum.IA;
                    }

                    if (plano.TryGetProperty("PlanoAtivoId", out var planoAtivoId))
                    {                         
                        usuario.Atributos.Plano.PlanoAtivoId = planoAtivoId.GetString();
                    }                    
                }                
            }            

            return usuario;
        }
    }
}
