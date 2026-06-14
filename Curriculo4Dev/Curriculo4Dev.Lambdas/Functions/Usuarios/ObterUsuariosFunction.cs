using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Curriculo4Dev.Core.Domain.AppService;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace Curriculo4Dev.Lambdas.Functions.Usuarios
{
    public class ObterUsuariosFunction : BaseFunction
    {
        public override async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var usuarioAppService = ServiceProvider.GetRequiredService<IUsuarioAppService>();

            var result = await usuarioAppService.ObterTodos();
            var jsonResponse = JsonSerializer.Serialize(result);

            return APIGatewayResponse.CreateResponse(jsonResponse, result.StatusCode);
        }
    }
}
