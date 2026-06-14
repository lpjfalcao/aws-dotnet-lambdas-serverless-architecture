using Curriculo4Dev.Core.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Curriculo4Dev.Infra.IoC;
using Microsoft.Extensions.Configuration;

namespace Curriculo4Dev.Lambdas
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UsuarioMappingProfile>();
                cfg.AddProfile<TemplateMappingProfile>();
            });

            services.ConfigureAppServices();
            services.ConfigureServices();
            services.ConfigureRepositories();
            services.ConfigureDatabase(configuration);
            services.ConfigureFactories();
            services.ConfigureFluentValidation();
        }
    }
}
