using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Curriculo4Dev.Core.Application.Factrories;
using Curriculo4Dev.Core.Domain.AppService;
using Curriculo4Dev.Core.Domain.Repositories;
using Curriculo4Dev.Core.Domain.Services;
using Curriculo4Dev.Core.Domain.Validators.Usuarios;
using Curriculo4Dev.Infra.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Curriculo4Dev.Infra.IoC
{
    public static class IoC
    {
        public static void ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppServiceBase<>), typeof(ServiceBase<>));            
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IPlanoService, PlanoService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string ambiente = Environment.GetEnvironmentVariable("AMBIENTE") ?? "dev";

            var config = new DynamoDBContextConfig
            {
                TableNamePrefix = $"{ambiente}-"
            };

            var awsOptions = configuration.GetAWSOptions();

            services.AddAWSService<IAmazonDynamoDB>(awsOptions);

            services.AddScoped<IDynamoDBContext>(provider =>
            {
                var dynamoClient = provider.GetRequiredService<IAmazonDynamoDB>();

                return new DynamoDBContext(dynamoClient);
            });
        }

        public static void ConfigureFactories(this IServiceCollection services)
        {
            services.AddScoped<IAppServiceFactory, AppServiceFactory>();
        }

        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddScoped<CadastrarAtualizarUsuarioValidator>();
            services.AddScoped<AssinarPlanoUsuarioValidator>();
        }
    }
}
