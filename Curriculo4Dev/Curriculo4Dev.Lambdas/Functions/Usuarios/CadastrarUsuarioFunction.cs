using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Curriculo4Dev.Core.Domain.AppService;
using Curriculo4Dev.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Curriculo4Dev.Lambdas.Functions.Usuarios
{   
    public class CadastrarUsuarioFunction : BaseFunction
    {
        public override async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var appService = ServiceProvider.GetRequiredService<IAppServiceBase<Usuario>>();

            var result = await appService.Create(request.Body);
            var jsonResponse = JsonSerializer.Serialize(result);

            return APIGatewayResponse.CreateResponse(jsonResponse, result.StatusCode);
        }
    }
}

