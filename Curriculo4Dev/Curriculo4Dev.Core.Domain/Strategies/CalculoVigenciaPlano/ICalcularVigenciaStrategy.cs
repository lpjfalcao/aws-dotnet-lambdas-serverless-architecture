namespace Curriculo4Dev.Core.Domain.Strategies.CalculoVigenciaPlano
{
    public interface ICalcularVigenciaStrategy
    {
        (DateTime dataVigenciaInicial, DateTime dataVigenciaFinal) CalcularVigencia();
    }
}
