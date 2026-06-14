using Amazon.Lambda.Core;
using AutoMapper;
using Curriculo4Dev.Core.Domain.AppService;
using Curriculo4Dev.Core.Domain.Common;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Exceptions;
using Curriculo4Dev.Core.Domain.Factories;
using Curriculo4Dev.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Curriculo4Dev.Core.Application.AppServices
{
    public class AppServiceBase<TEntity>(IServiceProvider serviceProvider, ILambdaContext context) : IAppServiceBase<TEntity> where TEntity : BaseEntity
    {
        private readonly IServiceBase<TEntity> serviceBase = serviceProvider.GetRequiredService<IServiceBase<TEntity>>();
        private readonly IMapper mapper = serviceProvider.GetRequiredService<IMapper>()!;
        private readonly ILambdaLogger logger = context.Logger;

        public async Task<MessageHelper> Create(string jsonRequest)
        {
            var result = new MessageHelper();

            try
            {
                logger.LogInformation("Processando a solicitação...");

                if (jsonRequest is null)
                {
                    result.BadRequest("O objeto enviado na requisição não pode ser nulo");

                    return result;
                }

                var entidadeNova = JsonSerializer.Deserialize<TEntity>(jsonRequest) ?? throw new Curriculo4DevException($"Não foi possível deserializar o JSON para o tipo {typeof(TEntity)}");

                entidadeNova.Id ??= PKKeyFactory.CreateKey(typeof(TEntity));
                entidadeNova.SortKey = SortKeyFactory.CreateKey(typeof(TEntity));
                entidadeNova.DataCadastro = DateTime.UtcNow;
                entidadeNova.DataAtualizacao = DateTime.UtcNow;

                logger.LogInformation($"Cadastrando entidade no DynamoDb com os dados recebidos: {JsonSerializer.Serialize(entidadeNova)}");

                await serviceBase.Create(entidadeNova);

                result.Ok();

                logger.LogInformation($"Retornando resposta para o client: {JsonSerializer.Serialize(result)}");

            }
            catch (Exception ex)
            {
                logger.LogError($"Ocorreu um erro durante o cadastro: {ex.Message}");

                result.Error(ex);
            }

            return result;
        }

        public async Task<MessageHelper<IEnumerable<Dto>>> GetAll<Dto>()
        {
            var result = new MessageHelper<IEnumerable<Dto>>();

            try
            {
                logger.LogInformation("Processando a solicitação GetAll()...");

                var entities = await serviceBase.GetAll();

                if (!entities.Any())
                {
                    logger.LogInformation("Nenhum registro foi encontrado");

                    result.NotFound("Nenhum dado foi encontrado");

                    return result;
                }

                result.Ok(mapper.Map<IEnumerable<Dto>>(entities));

                logger.LogInformation($"Retornando resposta para o client: {JsonSerializer.Serialize(result)}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocorreu um erro: {ex.Message}");

                result.Error(ex);
            }

            return result;
        }

        public async Task<MessageHelper<TDto>> GetByPrimaryKey<TDto>(string partitionKey, string sortKey)
        {
            var result = new MessageHelper<TDto>();

            try
            {
                logger.LogInformation($"Processando a solicitação...");

                var entity = await serviceBase.GetByPrimaryKey(partitionKey, sortKey);

                if (entity is null)
                {
                    result.NotFound($"Nenhum dado foi encontrado com a chave informada");

                    return result;
                }

                result.Ok(mapper.Map<TDto>(entity));

                logger.LogInformation($"Retornando resposta para o client: {JsonSerializer.Serialize(result)}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocorreu um erro: {ex.Message}");

                result.Error(ex);
            }

            return result;
        }
        public async Task<MessageHelper<IEnumerable<TDto>>> GetByPartitionKey<TDto>(string partitionKey)
        {
            var result = new MessageHelper<IEnumerable<TDto>>();

            try
            {
                logger.LogInformation($"Processando a solicitação...");

                var entity = await serviceBase.GetByPartitionKey(partitionKey);

                if (entity is null)
                {
                    result.NotFound($"Nenhum dado foi encontrado com a chave informada");

                    return result;
                }

                result.Ok(mapper.Map<IEnumerable<TDto>>(entity));

                logger.LogInformation($"Retornando resposta para o client: {JsonSerializer.Serialize(result)}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocorreu um erro: {ex.Message}");

                result.Error(ex);
            }

            return result;
        }


        public async Task<MessageHelper> Remove(string partitionKey, string sortKey)
        {
            var result = new MessageHelper();

            try
            {
                logger.LogInformation($"Processando a solicitação...");

                var entity = await serviceBase.GetByPrimaryKey(partitionKey, sortKey);

                if (entity is null)
                {
                    result.NotFound($"Nenhum dado foi encontrado com a chave informada");

                    return result;
                }

                logger.LogInformation("Removendo registro do banco de dados...");

                await serviceBase.Remove(entity);

                result.Ok();

                logger.LogInformation($"Retornando resposta para o client: {JsonSerializer.Serialize(result)}");

            }
            catch (Exception ex)
            {
                logger.LogError($"Ocorreu um erro: {ex.Message}");

                result.Error(ex);
            }

            return result;
        }

        public async Task<MessageHelper> Update(string partitionKey, string sortKey, string jsonRequest)
        {
            var result = new MessageHelper();

            try
            {
                logger.LogInformation($"Processando a solicitação com os dados recebidos: {JsonSerializer.Serialize(jsonRequest)}");

                var entidadeCadastradaNoDynamo = await serviceBase.GetByPrimaryKey(partitionKey, sortKey);

                if (entidadeCadastradaNoDynamo is null)
                {
                    result.NotFound($"Nenhum dado foi encontrado no Dynamo com a chave informada");

                    return result;
                }

                var entityMapper = EntityMapperFactory<TEntity>.Create();

                var entidadeNova = entityMapper.MapFrom(jsonRequest, entidadeCadastradaNoDynamo);

                logger.LogInformation($"Fazendo update dos dados da entidade no Dynamo com: {entidadeNova}");

                await serviceBase.Update(entidadeNova);

                result.Ok();

                logger.LogInformation($"Retornando resposta para o client: {JsonSerializer.Serialize(result)}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocorreu um erro: {ex.Message}");

                result.Error(ex);
            }

            return result;
        }
    }
}
