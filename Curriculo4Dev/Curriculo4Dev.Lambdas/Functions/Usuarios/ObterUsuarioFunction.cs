using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Curriculo4Dev.Core.Domain.AppService;
using Curriculo4Dev.Core.Domain.DataTransferObjects;
using Curriculo4Dev.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Curriculo4Dev.Lambdas.Functions.Usuarios
{
    public class ObterUsuarioFunction : BaseFunction
    {
        public override async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var appService = ServiceProvider.GetRequiredService<IAppServiceBase<Usuario>>();

            request.QueryStringParameters.TryGetValue("partitionKey", out var partitionKey);
            request.QueryStringParameters.TryGetValue("sortKey", out var sortKey);

            APIGatewayRequest.ValidarChaveParticao(partitionKey, sortKey);

            var result = await appService.GetByPrimaryKey<UsuarioGetDto>(partitionKey!, sortKey!);
            var jsonResponse = JsonSerializer.Serialize(result);

            return APIGatewayResponse.CreateResponse(jsonResponse, result.StatusCode);
        }
    }
}
