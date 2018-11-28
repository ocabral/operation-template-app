using FluentAssertions;
using Force.DeepCloner;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.HealthCheck;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Should().BeNullOrEmpty();
            response.Components.Count().Should().Be(1);
        }

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessTrue_WhenInitializeWithOneUnitOfWorkAndApplicationTypeNotNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            unitOfWorkMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, unitOfWorkMock.Object, ApplicationType.Other);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.Other);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Should().BeNullOrEmpty();
            response.Components.Count().Should().Be(1);
        }

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessTrue_WhenInitializeWithListOfUnitOfWorkAndNullApplicationType()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            unitOfWorkMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            Mock<IUnitOfWork> unitOfWorkMock2 = new Mock<IUnitOfWork>(MockBehavior.Strict);
            unitOfWorkMock2
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            IList<IUnitOfWork> unitOfWorkList = new List<IUnitOfWork>()
            {
                unitOfWorkMock.Object,
                unitOfWorkMock2.Object,
            };

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, unitOfWorkList);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Should().BeNullOrEmpty();
            response.Components.Count().Should().Be(2);
        }

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessTrue_WhenInitializeWithListOfRepositoriesAndNullApplicationType()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IRepository> repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            Mock<IRepository> repositoryMock2 = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock2
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            IList<IRepository> repositoryList = new List<IRepository>()
            {
                repositoryMock.Object,
                repositoryMock2.Object,
            };

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, repositoryList);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Should().BeNullOrEmpty();
            response.Components.Count().Should().Be(2);
        }

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessTrue_WhenInitializeWithListOfRepositoriesAndApplicationTypeNotNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IRepository> repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            Mock<IRepository> repositoryMock2 = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock2
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            IList<IRepository> repositoryList = new List<IRepository>()
            {
                repositoryMock.Object,
                repositoryMock2.Object,
            };

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, repositoryList, ApplicationType.QueueService);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.QueueService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Should().BeNullOrEmpty();
            response.Components.Count().Should().Be(2);
        }

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessFalse_WhenInitializeWithListOfRepositoriesAndNullApplicationType_AndAtLeastOneComponentStatusIsCritical()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IRepository> repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            Mock<IRepository> repositoryMock2 = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock2
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            var componentInfoWithError = appComponentInfoTest.DeepClone();
            componentInfoWithError.Status = Model.ApplicationStatus.Critical;

            Mock<IRepository> repositoryMock3 = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock3
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(componentInfoWithError));

            IList<IRepository> repositoryList = new List<IRepository>()
            {
                repositoryMock.Object,
                repositoryMock2.Object,
                repositoryMock3.Object,
            };

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, repositoryList);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
            response.Success.Should().BeFalse();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Components.Count().Should().Be(3);
        }

        [Fact]
        public async void GetHealthCheck_ShouldReturnSuccessFalse_WhenInitializeWithListOfRepositoriesAndNullApplicationType_AndAtLeastOneComponentStatusIsPartiallyAvailable()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IRepository> repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(appComponentInfoTest));

            var componentInfoWithError = appComponentInfoTest.DeepClone();
            componentInfoWithError.Status = Model.ApplicationStatus.PartiallyAvailable;

            Mock<IRepository> repositoryMock2 = new Mock<IRepository>(MockBehavior.Strict);
            repositoryMock2
                .Setup(x => x.GetDatabaseInfo())
                .Returns(Task.FromResult(componentInfoWithError));

            IList<IRepository> repositoryList = new List<IRepository>()
            {
                repositoryMock.Object,
                repositoryMock2.Object,
            };

            GetHealthCheckRequest request = new GetHealthCheckRequest();

            GetHealthCheck getHealthCheck = new GetHealthCheck(loggerMock.Object, repositoryList);

            // Act
            var response = await getHealthCheck.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
            response.Success.Should().BeFalse();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Components.Count().Should().Be(2);
        }
    }
}
