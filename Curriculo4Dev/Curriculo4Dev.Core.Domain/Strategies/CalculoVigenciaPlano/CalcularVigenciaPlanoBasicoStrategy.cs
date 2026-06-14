namespace Curriculo4Dev.Core.Domain.Strategies.CalculoVigenciaPlano
{
    public class CalcularVigenciaPlanoBasicoStrategy : ICalcularVigenciaStrategy
    {
        public (DateTime dataVigenciaInicial, DateTime dataVigenciaFinal) CalcularVigencia()
        {
            var dataVigenciaInicial = DateTime.UtcNow;
            var dataVigenciaFinal = dataVigenciaInicial.AddDays(30);

            return (dataVigenciaInicial, dataVigenciaFinal);
        }
    }
}
