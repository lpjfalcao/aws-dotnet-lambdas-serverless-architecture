using Amazon.Lambda.Core;
using AutoMapper;
using Curriculo4Dev.Core.Application.Extensions;
using Curriculo4Dev.Core.Domain.AppService;
using Curriculo4Dev.Core.Domain.Common;
using Curriculo4Dev.Core.Domain.DataTransferObjects;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Exceptions;
using Curriculo4Dev.Core.Domain.Factories;
using Curriculo4Dev.Core.Domain.Mappers;
using Curriculo4Dev.Core.Domain.Services;
using Curriculo4Dev.Core.Domain.Validators.Usuarios;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Curriculo4Dev.Core.Application.AppServices
{
    public class UsuarioAppService(IServiceProvider serviceProvider, ILambdaContext context) : AppServiceBase<Usuario>(serviceProvider, context), IUsuarioAppService
    {
        private readonly IUsuarioService usuarioService = serviceProvider.GetRequiredService<IUsuarioService>();
        private readonly IPlanoService planoService = serviceProvider.GetRequiredService<IPlanoService>();
        private readonly IMapper mapper = serviceProvider.GetRequiredService<IMapper>();
        private readonly IValidator<Usuario> assinarPlanoUsuarioValidator = serviceProvider.GetRequiredService<AssinarPlanoUsuarioValidator>();

        public async Task<MessageHelper<IEnumerable<UsuarioGetDto>>> ObterTodos(string sortKey = "PROFILE")
        {
            var message = new MessageHelper<IEnumerable<UsuarioGetDto>>();

            try
            {
                context.Logger.LogInformation("Buscando dados do Usuário no DynamoDb");

                var usuarios = await usuarioService.ObterTodos(sortKey);

                if (usuarios is null)
                {
                    message.NotFound("Nenhum usuário foi encontrado no banco de dados");

                    context.Logger.LogInformation("Nenhum usuário foi encontrado no banco de dados");

                    return message;
                }

                message.Ok(this.mapper.Map<IEnumerable<UsuarioGetDto>>(usuarios));
            }
            catch (Exception ex)
            {
                message.Error(ex);

                context.Logger.LogError($"Ocorreu um erro ao buscar os dados do usuário: {ex.Message}");
            }

            return message;
        }

        public async Task<MessageHelper> AssinarPlano(string usuarioJsonRequest, string partitionKey, string sortKey)
        {
            var message = new MessageHelper();

            try
            {
                context.Logger.LogInformation($"Iniciando a contratação do plano para o usuário - id do usuário: {partitionKey}:{sortKey}");

                var usuarioCadastrado = await usuarioService.GetByPrimaryKey(partitionKey, sortKey);

                if (usuarioCadastrado is null)
                {
                    context.Logger.LogInformation($"Não foram encontrado dados do usuário com as chaves: {partitionKey}:{sortKey}");

                    message.NotFound($"Não foram encontrado dados do usuário com as chaves: {partitionKey}:{sortKey}");

                    return message;
                }

                if (usuarioCadastrado.Atributos?.Plano?.IsActive() == true)
                {
                    context.Logger.LogInformation("O usuário já tem um plano contratado. Use a funcionalidade de upgrade para solicitar outro plano");

                    message.BadRequest("O usuário já tem um plano contratado. Use a funcionalidade de upgrade para solicitar outro plano");

                    return message;
                }

                context.Logger.LogInformation("Mapeando os dados da requisição para a entidade");

                IEntityMapper<Usuario> entityMapper = EntityMapperFactory<Usuario>.Create();

                Usuario usuarioComPlanoAssinado = entityMapper.MapFrom(usuarioJsonRequest, usuarioCadastrado);

                string planoId = usuarioComPlanoAssinado?.Atributos?.Plano?.PlanoAtivoId ?? throw new Curriculo4DevException("Não foi possível recuperar o Id do plano"); 

                Plano planoExistenteCadastrado = await planoService.GetByPrimaryKey("PLANOS", planoId);

                if (planoExistenteCadastrado is null)
                {
                    context.Logger.LogInformation($"Não foi encontrado um plano com a chave: {planoId}:PLANOS");

                    message.NotFound($"Não foi encontrado um plano com a chave: {planoId}:PLANOS");

                    return message;
                }

                ValidationResult validationResult = await assinarPlanoUsuarioValidator.ValidateAsync(usuarioComPlanoAssinado);

                if (!validationResult.IsValid)
                {
                    var errorMessage = validationResult.Errors.GetErrorMessage();
                    
                    context.Logger.LogInformation($"Ocorreu um ou mais erros na validação dos dados: {errorMessage}");

                    message.BadRequest(validationResult.Errors);

                    return message;
                }

                context.Logger.LogInformation($"Finalizando a contratação do plano de assinatura - id do plano: {usuarioComPlanoAssinado?.Atributos?.Plano?.PlanoAtivoId}");

                await usuarioService.AssinarPlano(usuarioComPlanoAssinado!);

                message.Ok();
            }
            catch(Curriculo4DevException ex)
            {
                context.Logger.LogError(ex.Message);

                message.Error(ex);
            }

            catch (Exception ex)
            {
                context.Logger.LogError($"Ocorreu um erro ao processar a solicitação: {ex.Message}");

                message.Error(ex);
            }

            return message;
        }

        public async Task<MessageHelper> CancelarPlano(string partitionKey, string sortKey, string planoAtivodId)
        {
            var message = new MessageHelper();

            try
            {
                var usuarioCadastrado = await usuarioService.GetByPrimaryKey(partitionKey, sortKey);

                if (usuarioCadastrado is null)
                {
                    context.Logger.LogInformation($"Não foram encontrado dados do usuário com as chaves: {partitionKey}:{sortKey}");

                    message.NotFound($"Não foram encontrado dados do usuário com as chaves: {partitionKey}:{sortKey}");

                    return message;
                }

                if (usuarioCadastrado.Atributos?.Plano?.IsActive() == true)
                {
                    usuarioCadastrado.Atributos.Plano.DataFimVigenciaPlano = DateTime.UtcNow;
                }

                message.Ok();

            }
            catch(Exception ex)
            {
                message.Error(ex);
            }

            return message;
        }
    }
}
