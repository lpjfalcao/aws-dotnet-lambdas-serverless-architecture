using Curriculo4Dev.Core.Application.AppServices;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Tests.Fixtures;
using Moq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Curriculo4Dev.Tests.AppServices
{
    public class DocumentoAppServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CommonFixture _commonFixture;

        public DocumentoAppServiceTests(CommonFixture commonFixture)
        {
            _commonFixture = commonFixture;
        }

        [Fact]
        public async Task CreateDocument_WhenCalled_ShouldCreateEntity()
        {
            // Arrange
            var atributos = new JsonObject
            {
                { "ConfiguracaoJson", "{\"bold\" : true}" },
                { "Titulo", "Meu currículo" },
                { "TemplateId", "TEMPLATE#b024ccdc-69fb-4d46-a5f6-d551ba17be4a" }
            };

            var documento = new JsonObject
            {
                { "Id", "USER#bb228fa1-3b59-45f4-9bd4-253df4ca26d8" },
                { "Atributos",  atributos }
            };

            var appService = new AppServiceBase<Documento>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.Create(JsonSerializer.Serialize(documento));

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