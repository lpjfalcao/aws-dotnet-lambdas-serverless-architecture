using Curriculo4Dev.Core.Application.AppServices;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Enums;
using Curriculo4Dev.Core.Domain.Services;
using Curriculo4Dev.Core.Domain.Validators.Usuarios;
using Curriculo4Dev.UnitTests.Fixtures;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Curriculo4Dev.UnitTests.AppServices
{
    public class UsuarioAppServiceTests : IClassFixture<UsuarioAppServiceFixture>
    {
        private readonly UsuarioAppServiceFixture appServiceFixture;

        public UsuarioAppServiceTests(UsuarioAppServiceFixture appServiceFixture)
        {
            this.appServiceFixture = appServiceFixture;
        }

        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.Basico)]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.IA)]
        public async Task AssinarPlano_QuandoUsuarioNaoTemPlano_RetornaSucesso(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();
            var validationResult = new FluentValidation.Results.ValidationResult();

            var usuario = new Usuario
            {
                Id = "1234",
                SortKey = "12345",
                Atributos = new UsuarioAtributos()
            };

            var plano = new Plano
            {
                Atributos = new PlanoAtributos
                {
                    TipoPlano = new TipoPlano
                    {
                        Codigo = tipoPlano,
                        Nome = "Básico"
                    },
                    Descricao = "descrição do plano",
                    Nome = "Plano Básico",
                    Preco = 100M,
                    Recursos = ["A", "B", "C"]
                },
                Id = "123465",
                SortKey = "123456789"
            };

            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7" },
                                { "TipoPlano", (int) tipoPlano }
                            }
                }
            };

            var usuarioRequest = new JsonObject
            {
                { "Atributos",  atributos }
            };

            assinarPlanoUsuarioValidator
                .As<IValidator<Usuario>>()
                .Setup(x => x.ValidateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(usuario);
            usuarioService.Setup(x => x.AssinarPlano(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
            planoService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(plano);

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            usuarioService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            usuarioService.Verify(x => x.AssinarPlano(It.IsAny<Usuario>()), Times.Exactly(1));
            planoService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(3));

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Success);
            Assert.Equal("Operação realizada com sucesso!", result.Message);
        }

        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.Basico)]
        public async Task AssinarPlano_QuandoUsuarioNaoForEncontrado_RetornaFalha(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();

            string planoId = "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7";

            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", planoId },
                                { "TipoPlano", (int) tipoPlano }
                            }
                }
            };

            var usuarioRequest = new JsonObject
            {
                { "Atributos",  atributos }
            };

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Usuario)null!);

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            usuarioService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(2));

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal($"Não foram encontrado dados do usuário com as chaves: {partitionKey}:{sortKey}", result.Message);
        }

        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.Basico)]
        public async Task AssinarPlano_QuandoUsuarioJaPossuiUmPlano_RetornaFalha(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();

            string planoId = "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7";

            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", planoId },
                                { "TipoPlano", (int) tipoPlano }
                            }
                }
            };

            var usuario = new Usuario
            {
                Id = "1234",
                SortKey = "12345",
                Atributos = new UsuarioAtributos
                {
                    Plano = new PlanoContratado
                    {
                        PlanoAtivoId = planoId,
                        TipoPlano = TipoPlanoEnum.Basico,
                        DataFimVigenciaPlano = DateTime.UtcNow.AddDays(30),
                        DataInicioVigenciaPlano = DateTime.UtcNow
                    }
                }
            };

            var usuarioRequest = new JsonObject
            {
                { "Atributos",  atributos }
            };

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(usuario);

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            usuarioService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(2));

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal("O usuário já tem um plano contratado. Use a funcionalidade de upgrade para solicitar outro plano", result.Message);
        }

        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.IA)]
        public async Task AssinarPlano_QuandoNaoForInformadoPlanoAtivoId_RetornaFalha(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();
            var validationResult = new FluentValidation.Results.ValidationResult();

            var usuario = new Usuario
            {
                Id = "1234",
                SortKey = "12345",
                Atributos = new UsuarioAtributos()
            };

            var plano = new Plano
            {
                Atributos = new PlanoAtributos
                {
                    TipoPlano = new TipoPlano
                    {
                        Codigo = tipoPlano,
                        Nome = "Básico"
                    },
                    Descricao = "descrição do plano",
                    Nome = "Plano Básico",
                    Preco = 100M,
                    Recursos = ["A", "B", "C"]
                },
                Id = "123465",
                SortKey = "123456789"
            };

            var usuarioRequest = new JsonObject();

            assinarPlanoUsuarioValidator
                .As<IValidator<Usuario>>()
                .Setup(x => x.ValidateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(usuario);

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            usuarioService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(2));
            loggerMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Exactly(1));

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal("Não foi possível recuperar o Id do plano", result.Message);
        }

        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.IA)]
        public async Task AssinarPlano_QuandoPlanoSelecionadoNaoExisteNoSistema_RetornaFalha(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();
            var validationResult = new FluentValidation.Results.ValidationResult();

            string planoId = "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7";

            var usuario = new Usuario
            {
                Id = "1234",
                SortKey = "12345",
                Atributos = new UsuarioAtributos()
            };

            var plano = new Plano
            {
                Atributos = new PlanoAtributos
                {
                    TipoPlano = new TipoPlano
                    {
                        Codigo = tipoPlano,
                        Nome = "Básico"
                    },
                    Descricao = "descrição do plano",
                    Nome = "Plano Básico",
                    Preco = 100M,
                    Recursos = ["A", "B", "C"]
                },
                Id = "123465",
                SortKey = "123456789"
            };

            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", planoId },
                                { "TipoPlano", (int) tipoPlano }
                            }
                }
            };

            var usuarioRequest = new JsonObject
            {
                { "Atributos",  atributos }
            };

            assinarPlanoUsuarioValidator
                .As<IValidator<Usuario>>()
                .Setup(x => x.ValidateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(usuario);
            usuarioService.Setup(x => x.AssinarPlano(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
            planoService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Plano) null!);

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            usuarioService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            planoService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(3));

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal($"Não foi encontrado um plano com a chave: {planoId}:PLANOS", result.Message);
        }


        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.Basico)]
        public async Task AssinarPlano_QuandoDadosUsuarioSaoInvalidos_RetornaFalhao(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();
            var validationResult = new FluentValidation.Results.ValidationResult();

            validationResult.Errors.Add(new ValidationFailure { ErrorCode = "Teste", ErrorMessage = "Erro de validação" });
            validationResult.Errors.Add(new ValidationFailure { ErrorCode = "Teste", ErrorMessage = "Erro de validação 2" });
            validationResult.Errors.Add(new ValidationFailure { ErrorCode = "Teste", ErrorMessage = "Erro de validação 4" });

            var usuario = new Usuario
            {
                Id = "1234",
                SortKey = "12345",
                Atributos = new UsuarioAtributos()
            };

            var plano = new Plano
            {
                Atributos = new PlanoAtributos
                {
                    TipoPlano = new TipoPlano
                    {
                        Codigo = tipoPlano,
                        Nome = "Básico"
                    },
                    Descricao = "descrição do plano",
                    Nome = "Plano Básico",
                    Preco = 100M,
                    Recursos = ["A", "B", "C"]
                },
                Id = "123465",
                SortKey = "123456789"
            };

            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7" },
                                { "TipoPlano", (int) tipoPlano }
                            }
                }
            };

            var usuarioRequest = new JsonObject
            {
                { "Atributos",  atributos }
            };

            assinarPlanoUsuarioValidator
                .As<IValidator<Usuario>>()
                .Setup(x => x.ValidateAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(usuario);
            usuarioService.Setup(x => x.AssinarPlano(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
            planoService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(plano);

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            usuarioService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            planoService.Verify(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeast(3));

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.False(result.Success);
            Assert.Equal("Ocorreu um ou mais erros de validação. Veja os detalhes na propriedade 'Validations'", result.Message);
        }


        [Theory]
        [InlineData("USER#f2c1a5d6-3a61-4448-a952-601838928d1c", "PROFILE", TipoPlanoEnum.Basico)]
        public async Task AssinarPlano_QuandoExcecaoForLancada_RetornaFalha(string partitionKey, string sortKey, TipoPlanoEnum tipoPlano)
        {
            // Arrange
            var usuarioService = new Mock<IUsuarioService>();
            var planoService = new Mock<IPlanoService>();
            var assinarPlanoUsuarioValidator = new Mock<AssinarPlanoUsuarioValidator>();

            string planoId = "PLANO#24a006d5-56a9-4fe9-8a17-cecbce854dc7";

            var atributos = new JsonObject
            {
                { "Plano", new JsonObject
                            {
                                { "PlanoAtivoId", planoId },
                                { "TipoPlano", (int) tipoPlano }
                            }
                }
            };

            var usuarioRequest = new JsonObject
            {
                { "Atributos",  atributos }
            };

            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IUsuarioService))).Returns(usuarioService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(IPlanoService))).Returns(planoService.Object);
            this.appServiceFixture.ServiceProvider.Setup(x => x.GetService(typeof(AssinarPlanoUsuarioValidator))).Returns(assinarPlanoUsuarioValidator.Object);

            usuarioService.Setup(x => x.GetByPrimaryKey(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            var loggerMock = Mock.Get(this.appServiceFixture.LambdaContext.Object.Logger);

            var appService = new UsuarioAppService(this.appServiceFixture.ServiceProvider.Object, this.appServiceFixture.LambdaContext.Object);

            var usuarioJsonRequest = JsonSerializer.Serialize(usuarioRequest);

            // Act            
            var result = await appService.AssinarPlano(usuarioJsonRequest, partitionKey, sortKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.False(result.Success);
        }
    }
}
