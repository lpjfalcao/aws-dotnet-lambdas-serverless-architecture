using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Curriculo4Dev.Lambdas.Functions
{
    public abstract class BaseFunction
    {
        protected readonly IServiceProvider ServiceProvider;

        public abstract Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context);

        public BaseFunction()
        {
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Startup.ConfigureServices(serviceCollection, configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
