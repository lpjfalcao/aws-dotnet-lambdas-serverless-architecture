using Curriculo4Dev.Core.Application.AppServices;
using Curriculo4Dev.Core.Domain.DataTransferObjects;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Enums;
using Curriculo4Dev.Tests.Fixtures;
using Moq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Curriculo4Dev.Tests.AppServices
{
    public class UsuarioAppServiceBaseTests : IClassFixture<CommonFixture>
    {
        private readonly CommonFixture _commonFixture;

        public UsuarioAppServiceBaseTests(CommonFixture commonFixture)
        {
            _commonFixture = commonFixture;
        }

        [Fact]
        public async Task Cadastrar_DeveCadastrarUsuarioNoDynamo()
        {
            // Arrange
            var atributos = new JsonObject
            {
                { "Nome", "Tony Stark" },
                { "Username", "tony-stark" },
                { "Email", "tonystartk@gmail.com" }
            };

            var usuario = new JsonObject
            {
                { "Atributos",  atributos }
            };

            var appService = new AppServiceBase<Usuario>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.Create(JsonSerializer.Serialize(usuario));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarTodosUsuariosCadastrados()
        {
            // Arrange
            var appService = new UsuarioAppService(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.ObterTodos();

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(1));

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Theory]
        [InlineData("USER#3e35007c-2988-4e40-974b-ae86d34e3f08", "PROFILE")]
        public async Task ObterUsuarioPorPk_DeveRetornarEntidadeComaPKInformada(string partitionKey, string sortKey)
        {
            // Arrange
            var appService = new AppServiceBase<Usuario>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.GetByPrimaryKey<UsuarioGetDto>(partitionKey, sortKey);

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(2));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Data.Id, partitionKey);
            Assert.Equal(result.Data.SortKey, sortKey);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Theory]
        [InlineData("USER#3e35007c-2988-4e40-974b-ae86d34e3f08", "PROFILE")]
        public async Task AtualizarUsuario_DeveAtualizarUsuarioCadastradoNoDynamoComDadosInformados(string partitionKey, string sortKey)
        {
            // Arrange
            var atributos = new JsonObject
            {
                { "Email", "steve.rogers@avengers.com" }
            };

            var usuario = new JsonObject
            {
                { "Atributos",  atributos }
            };

            var appService = new AppServiceBase<Usuario>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.Update(partitionKey, sortKey, JsonSerializer.Serialize(usuario));

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(3));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);            
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE")]
        public async Task ContratarPlanoParaUsuario_DeveAtualizarAtributosUsuarioComDadosDoPlano(string partitionKey, string sortKey)
        {
            // Arrange
            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7" },
                                { "TipoPlano", (int) TipoPlanoEnum.Basico }
                            }
                }
            };

            var usuario = new JsonObject
            {
                { "Atributos",  atributos }
            };

            var appService = new UsuarioAppService(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.AssinarPlano(JsonSerializer.Serialize(usuario), partitionKey, sortKey);

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(2));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Theory]
        [InlineData("USER#3e35007c-2988-4e40-974b-ae86d34e3f08", "PROFILE")]
        public async Task RemoverUsuario_DeveExcluirUsuarioDoDynamo(string partitionKey, string sortKey)
        {
            var appService = new AppServiceBase<Usuario>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.Remove(partitionKey, sortKey);

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(3));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }
    }
}