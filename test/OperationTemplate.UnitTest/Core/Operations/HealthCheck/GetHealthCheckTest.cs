using FluentAssertions;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.HealthCheck;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Model = StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Operations.HealthCheck
{
    public class GetHealthCheckTest
    {
        private readonly Model.ApplicationComponentInfo appComponentInfoTest = new Model.ApplicationComponentInfo()
        {
            ApplicationName = "Database test",
            ApplicationType = Model.ApplicationType.Database,
            Timestamp = DateTime.UtcNow,
            MachineName = "Machine test",
            Status = Model.ApplicationStatus.Ok,
            Version = "1.2",
        };

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessTrue_WhenInitializeWithOneUnitOfWorkAndNullApplicationType()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            unitOfWorkMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, unitOfWorkMock.Object);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Should().BeNullOrEmpty();
            response.Components.Count().Should().Be(1);
        }
    }
}
