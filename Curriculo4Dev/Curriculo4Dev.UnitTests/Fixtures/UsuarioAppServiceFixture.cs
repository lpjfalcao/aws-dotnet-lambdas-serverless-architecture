using Amazon.Lambda.Core;
using AutoMapper;
using Curriculo4Dev.Core.Domain.Entities;
using Curriculo4Dev.Core.Domain.Services;
using Curriculo4Dev.Core.Domain.Validators.Usuarios;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace Curriculo4Dev.UnitTests.Fixtures
{
    public class UsuarioAppServiceFixture
    {
        public Mock<IServiceProvider> ServiceProvider { get; set; } = new();
        public Mock<ILambdaContext> LambdaContext { get; set; } = new();
        private Mock<ILambdaLogger> LambdaLogger { get; set; } = new();

        public UsuarioAppServiceFixture()
        {
            LambdaLogger.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<object>()));
            LambdaLogger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<object>()));
            LambdaContext.Setup(x => x.Logger).Returns(LambdaLogger.Object);

            var autoMapper = new Mock<IMapper>();
            var serviceBase = new Mock<IServiceBase<Usuario>>();

            ServiceProvider.Setup(x => x.GetService(typeof(IMapper))).Returns(autoMapper.Object);
            ServiceProvider.Setup(x => x.GetService(typeof(IServiceBase<Usuario>))).Returns(serviceBase.Object);
        }
    }
}
