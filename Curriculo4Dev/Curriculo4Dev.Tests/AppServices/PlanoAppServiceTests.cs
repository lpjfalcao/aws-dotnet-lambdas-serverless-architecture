using Curriculo4Dev.Core.Application.AppServices;
using Curriculo4Dev.Core.Application.DataTransferObjects;
using Curriculo4Dev.Core.Application.DataTransferObjects.Templates;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Enums;
using Curriculo4Dev.Tests.Fixtures;
using Moq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Curriculo4Dev.Tests.AppServices
{
    public class PlanoAppServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CommonFixture _commonFixture;

        public PlanoAppServiceTests(CommonFixture commonFixture)
        {
            _commonFixture = commonFixture;
        }

        [Fact]
        public async Task CadastrarPlano_DeveCriarPlanoNoDynamoDb()
        {
            // Arrange
            var atributos = new JsonObject
            {
                { "Nome", "Básico" },
                { "Descricao", "Adequado para quem deseja ter controle manual da criação do currículo" },
                { "Preco", 19.9 },
                { "TipoPlano", new JsonObject{ { "Codigo", 1 }, { "Nome", "Básico" }  }},
                { "Recursos", new JsonArray("Editor de currículo dinâmico e em tempo real", "Download em PDF ilimitado", "Templates personalizados para o seu momento de carreira") }
            };

            var plano = new JsonObject
            {
                { "Atributos",  atributos }
            };

            var appService = new AppServiceBase<Plano>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.Create(JsonSerializer.Serialize(plano));

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