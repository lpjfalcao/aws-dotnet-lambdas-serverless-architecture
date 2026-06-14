namespace Curriculo4Dev.Core.Domain.Strategies.CalculoVigenciaPlano
{
    public class CalcularVigenciaPlanoIAStrategy : ICalcularVigenciaStrategy
    {
        public (DateTime dataVigenciaInicial, DateTime dataVigenciaFinal) CalcularVigencia()
        {
            var dataVigenciaInicial = DateTime.UtcNow;
            var dataVigenciaFinal = dataVigenciaInicial.AddMonths(3);

            return (dataVigenciaInicial, dataVigenciaFinal);
        }
    }
}
