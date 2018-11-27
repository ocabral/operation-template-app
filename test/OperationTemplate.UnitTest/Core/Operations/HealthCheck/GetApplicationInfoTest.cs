using FluentAssertions;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.HealthCheck;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System.Collections.Generic;
using Xunit;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Operations.HealthCheck
{
    public class GetApplicationInfoTest
    {
        [Fact]
        public async void GetApplicationInfo_ShouldReturnSuccessTrueAndApplicationTypeWebService_WhenApplicationTypeNotInformed()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            GetApplicationInfoRequest request = new GetApplicationInfoRequest();

            GetApplicationInfo getAppInfo = new GetApplicationInfo(loggerMock.Object);

            // Act
            var response = await getAppInfo.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.WebService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.MachineName.Should().NotBeNullOrWhiteSpace();
            response.OS.Should().NotBeNull();
            response.Status.Should().BeEquivalentTo(ApplicationStatus.Ok);
            response.Version.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async void GetApplicationInfo_ShouldReturnSuccessTrueAndApplicationTypeWebService_WhenApplicationTypeInformed()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            GetApplicationInfoRequest request = new GetApplicationInfoRequest();

            GetApplicationInfo getAppInfo = new GetApplicationInfo(loggerMock.Object, ApplicationType.QueueService);

            // Act
            var response = await getAppInfo.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.Success.Should().BeTrue();
            response.ApplicationName.Should().NotBeNullOrWhiteSpace();
            response.ApplicationType.Should().BeEquivalentTo(ApplicationType.QueueService);
            response.BuildDate.ToString().Should().NotBeNullOrWhiteSpace();
            response.MachineName.Should().NotBeNullOrWhiteSpace();
            response.OS.Should().NotBeNull();
            response.Status.Should().BeEquivalentTo(ApplicationStatus.Ok);
            response.Version.Should().NotBeNullOrWhiteSpace();
        }
    }
}
