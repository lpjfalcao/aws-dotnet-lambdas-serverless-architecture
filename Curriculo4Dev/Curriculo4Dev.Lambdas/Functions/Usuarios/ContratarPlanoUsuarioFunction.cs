using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Curriculo4Dev.Core.Domain.AppService;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Curriculo4Dev.Lambdas.Functions.Usuarios
{
    public class ContratarPlanoUsuarioFunction : BaseFunction
    {
        public override async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var usuarioAppService = ServiceProvider.GetRequiredService<IUsuarioAppService>();

            request.QueryStringParameters.TryGetValue("partitionKey", out var partitionKey);
            request.QueryStringParameters.TryGetValue("sortKey", out var sortKey);

            APIGatewayRequest.ValidarChaveParticao(partitionKey, sortKey);
            
            var result = await usuarioAppService.AssinarPlano(request.Body, partitionKey!, sortKey!);
            var jsonResponse = JsonSerializer.Serialize(result);

            return APIGatewayResponse.CreateResponse(jsonResponse, result.StatusCode);
        }
    }
}
