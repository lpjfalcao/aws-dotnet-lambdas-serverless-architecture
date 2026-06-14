using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Curriculo4Dev.Lambdas.Functions.Transacoes
{
    public class AtualizarTransacaoFunction : BaseFunction
    {
        public override Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            throw new NotImplementedException();
        }
    }
}
