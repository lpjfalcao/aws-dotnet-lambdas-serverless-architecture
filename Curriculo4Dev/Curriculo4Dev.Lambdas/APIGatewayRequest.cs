using Curriculo4Dev.Core.Domain.Exceptions;

namespace Curriculo4Dev.Lambdas
{
    public class APIGatewayRequest
    {
        public static void ValidarChaveParticao(string? partitionKey, string? sortKey)
        {
            if (partitionKey is null || sortKey is null)
                throw new Curriculo4DevException("A chave primária composta é obrigatória e deve conter a partition key e a sort key");
        }
    }
}
