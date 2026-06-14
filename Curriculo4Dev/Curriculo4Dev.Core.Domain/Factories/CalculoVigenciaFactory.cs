using Curriculo4Dev.Core.Domain.Enums;
using Curriculo4Dev.Core.Domain.Strategies.CalculoVigenciaPlano;

namespace Curriculo4Dev.Core.Domain.Factories
{
    public class CalculoVigenciaFactory
    {
        public static ICalcularVigenciaStrategy Create(TipoPlanoEnum tipoPlano)
        {
            return tipoPlano switch
            {
                TipoPlanoEnum.Basico => new CalcularVigenciaPlanoBasicoStrategy(),
                TipoPlanoEnum.IA => new CalcularVigenciaPlanoIAStrategy(),
                _ => throw new ArgumentException("O plano fornecido não existe ou é inválido")
            };
        }
    }
}
