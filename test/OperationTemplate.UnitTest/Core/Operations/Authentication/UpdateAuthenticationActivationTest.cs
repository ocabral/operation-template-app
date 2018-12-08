using FluentAssertions;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Model = StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Operations.Authentication
{
    public class UpdateAuthenticationActivationTest
    {
        [Fact]
        public async void UpdateAuthenticationActivation_ShouldReturnSuccessTrue_WhenRequestIsValid()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.UpdateActivation(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(() => Task.FromResult(1));

            UpdateAuthenticationActivationRequest request = new UpdateAuthenticationActivationRequest
            {
                ApplicationKey = "Any app key",
                IsActive = false,
            };

            IUpdateAuthenticationActivation operation = new UpdateAuthenticationActivation(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.Errors.Should().BeNullOrEmpty();
            response.ApplicationKey.Should().NotBeNull();
            response.IsActive.Should().BeFalse();
        }

        [Fact]
        public async void UpdateAuthenticationActivation_ShouldReturnSuccessFalse_WhenRequestIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.UpdateActivation(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(() => Task.FromResult(0));

            UpdateAuthenticationActivationRequest request = null;

            IUpdateAuthenticationActivation operation = new UpdateAuthenticationActivation(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Should().NotBeNullOrEmpty();
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.ApplicationKey.Should().BeNull();
            response.IsActive.Should().BeNull();
        }

        [Fact]
        public async void UpdateAuthenticationActivation_ShouldReturnSuccessFalse_WhenApplicationKeyIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.UpdateActivation(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(() => Task.FromResult(0));

            UpdateAuthenticationActivationRequest request = new UpdateAuthenticationActivationRequest
            {
                ApplicationKey = null,
                IsActive = false,
            };

            IUpdateAuthenticationActivation operation = new UpdateAuthenticationActivation(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Should().NotBeNullOrEmpty();
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.ApplicationKey.Should().BeNull();
            response.IsActive.Should().BeNull();
        }

        [Fact]
        public async void UpdateAuthenticationActivation_ShouldReturnSuccessFalse_WhenApplicationKeyDoesNotExist()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.UpdateActivation(It.IsNotIn<string>("Valid app key"), It.IsAny<bool>()))
                .Returns(() => Task.FromResult(0));

            UpdateAuthenticationActivationRequest request = new UpdateAuthenticationActivationRequest
            {
                ApplicationKey = "Inexistent app key.",
                IsActive = false,
            };

            IUpdateAuthenticationActivation operation = new UpdateAuthenticationActivation(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
            response.Success.Should().BeFalse();
            response.Errors.Should().NotBeNullOrEmpty();
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.ApplicationKey.Should().BeNull();
            response.IsActive.Should().BeNull();
        }
    }
}
