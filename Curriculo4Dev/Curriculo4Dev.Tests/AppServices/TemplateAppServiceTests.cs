using Curriculo4Dev.Core.Application.AppServices;
using Curriculo4Dev.Core.Application.DataTransferObjects;
using Curriculo4Dev.Core.Application.DataTransferObjects.Templates;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Tests.Fixtures;
using Moq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Curriculo4Dev.Tests.AppServices
{
    public class TemplateAppServiceBaseTests : IClassFixture<CommonFixture>
    {
        private readonly CommonFixture _commonFixture;

        public TemplateAppServiceBaseTests(CommonFixture commonFixture)
        {
            _commonFixture = commonFixture;
        }      

        [Fact]
        public async Task CreateTemplate_WhenCalled_ShouldCreateEntity()
        {
            // Arrange
            var atributos = new JsonObject
            {
                { "Descricao", "Template para Dev Senior" },
                { "UrlImagemS3", "LINK_DA_IMAGEM_DO_TEMPLATE_AQUI" },
            };

            var template = new JsonObject
            {
                { "Atributos",  atributos }
            };

            var appService = new AppServiceBase<Template>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.Create(JsonSerializer.Serialize(template));

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(3));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Theory]
        [InlineData("TEMPLATES")]
        public async Task GetTemplates_WhenCalled_ShouldReturnEntities(string partitionKey)
        {
            // Arrange
            var appService = new AppServiceBase<Template>(_commonFixture.ServiceProvider, _commonFixture.LambdaContext.Object);

            // Act
            var result = await appService.GetByPartitionKey<TemplateGetDto>(partitionKey);

            var loggerMock = Mock.Get(_commonFixture.LambdaContext.Object.Logger);

            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(2));

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }
    }
}