using Amazon.Lambda.Core;
using Curriculo4Dev.Lambdas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Curriculo4Dev.Tests.Fixtures
{
    public class CommonFixture
    {
        public IServiceProvider ServiceProvider { get; set; }
        public Mock<ILambdaContext> LambdaContext { get; set; }        
        private Mock<ILambdaLogger> LambdaLogger { get; set; }

        public CommonFixture()
        {
            Environment.SetEnvironmentVariable("AMBIENTE", "dev");

            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Startup.ConfigureServices(serviceCollection, configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            LambdaLogger = new Mock<ILambdaLogger>();

            LambdaLogger.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<object>()));
            LambdaLogger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<object>()));

            LambdaContext = new Mock<ILambdaContext>();

            LambdaContext.Setup(x => x.Logger).Returns(LambdaLogger.Object);            
        }
    }
}
