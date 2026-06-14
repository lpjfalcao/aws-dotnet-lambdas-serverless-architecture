using Amazon.Lambda.Core;
using Curriculo4Dev.Core.Application.AppServices;
using Curriculo4Dev.Core.Application.Enums;
using Curriculo4Dev.Core.Domain.AppService;
using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Application.Factrories
{
    public interface IAppServiceFactory
    {
        IAppServiceBase<T> Create<T>(TipoAppServiceEnum tipoService, IServiceProvider serviceProvider, ILambdaContext context) where T : BaseEntity;
        dynamic Create(TipoAppServiceEnum tipoService, IServiceProvider serviceProvider, ILambdaContext context);
    }

    public class AppServiceFactory : IAppServiceFactory
    {
       public IAppServiceBase<T> Create<T>(TipoAppServiceEnum tipoService, IServiceProvider serviceProvider, ILambdaContext context) where T : BaseEntity
        {
            return tipoService switch
            {
                TipoAppServiceEnum.AppServiceBase => new AppServiceBase<T>(serviceProvider, context),
                _ => throw new ArgumentException("Não foi possível criar o AppService com os parâmetros informados")
            };
            
        }
        public dynamic Create(TipoAppServiceEnum tipoService, IServiceProvider serviceProvider, ILambdaContext context) 
        {
            return tipoService switch
            {
                TipoAppServiceEnum.UsuarioAppService => new UsuarioAppService(serviceProvider, context),
                _ => throw new ArgumentException("Não foi possível criar o AppService com os parâmetros informados")
            };

        }

    }
}
