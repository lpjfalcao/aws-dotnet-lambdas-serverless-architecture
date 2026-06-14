using Amazon.DynamoDBv2.DataModel;
using Curriculo4Dev.Core.Domain.Enums;
using Curriculo4Dev.Core.Domain.Factories;

namespace Curriculo4Dev.Core.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        [DynamoDBProperty("Atributos")]
        public UsuarioAtributos Atributos { get; set; } = new();
    }

    public class UsuarioAtributos
    {
        [DynamoDBProperty("Nome")]
        public string Nome { get; set; } = null!;

        [DynamoDBProperty("Username")]
        public string Username { get; set; } = null!;

        [DynamoDBProperty("Email")]
        public string Email { get; set; } = null!;

        [DynamoDBProperty("Documento")]
        public Documento? Documento { get; set; }
        public PlanoContratado? Plano { get; set; }
    }

    public class PlanoContratado
    {
        public string? PlanoAtivoId { get; set; }        
        public TipoPlanoEnum TipoPlano { get; set; }
        public DateTime? DataInicioVigenciaPlano { get; set; }
        public DateTime? DataFimVigenciaPlano { get; set; }

        public void CalcularVigencia()
        {
            if (PlanoAtivoId is not null)
            {
                var calculoVigenciaStrategy = CalculoVigenciaFactory.Create(TipoPlano);
                var (dataVigenciaInicial, dataVigenciaFinal) = calculoVigenciaStrategy.CalcularVigencia();

                DataInicioVigenciaPlano = dataVigenciaInicial;
                DataFimVigenciaPlano = dataVigenciaFinal;
            }
        }

        public bool IsActive()
        {
            return (PlanoAtivoId is not null && DataFimVigenciaPlano <= DateTime.Now);
        }
    }
}

