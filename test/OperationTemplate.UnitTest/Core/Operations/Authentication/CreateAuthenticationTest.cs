using FluentAssertions;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Model = StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Operations.Authentication
{
    public class CreateAuthenticationTest
    {
        private readonly Model.AuthenticationModel _preGeneratedAuthentication = new Model.AuthenticationModel()
        {
            ApplicationKey = Guid.NewGuid().ToString("N"),
            ApplicationName = "App unit test",
            ApplicationToken = CryptographyExtensions.GenerateRandomClientToken(),
            CreationDateTime = DateTimeOffset.UtcNow,
            IsActive = true,
            Id = 123
        };

        [Fact]
        public async void CreateAuthentication_ShouldReturnSuccessTrue_WhenRequestIsValid()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.Insert(It.IsAny<Model.AuthenticationModel>()))
                .Returns(() => Task.FromResult(this._preGeneratedAuthentication));

            CreateAuthenticationRequest request = new CreateAuthenticationRequest
            {
                ApplicationName = "Valid app"
            };

            ICreateAuthentication operation = new CreateAuthentication(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.Errors.Should().BeNullOrEmpty();
            response.Data.ApplicationKey.Should().NotBeNull();
            response.Data.ApplicationName.Should().NotBeNull();
            response.Data.ApplicationToken.Should().NotBeNull();
            response.Data.IsActive.Should().BeTrue();
        }

        [Fact]
        public async void CreateAuthentication_ShouldReturnSuccessFalse_WhenRequestIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.Insert(It.IsAny<Model.AuthenticationModel>()))
                .Returns(() => Task.FromResult(this._preGeneratedAuthentication));

            CreateAuthenticationRequest request = null;

            ICreateAuthentication operation = new CreateAuthentication(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.Data.Should().BeNull();
        }

        [Fact]
        public async void CreateAuthentication_ShouldReturnSuccessFalse_WhenApplicationNameIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.Insert(It.IsAny<Model.AuthenticationModel>()))
                .Returns(() => Task.FromResult(this._preGeneratedAuthentication));

            CreateAuthenticationRequest request = new CreateAuthenticationRequest
            {
                ApplicationName = null,
            };

            ICreateAuthentication operation = new CreateAuthentication(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.Data.Should().BeNull();
        }

        [Fact]
        public async void CreateAuthentication_ShouldReturnSuccessFalse_WhenRepositoryThrowsAnyException()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.Insert(It.IsAny<Model.AuthenticationModel>()))
                .Returns(() => throw new Exception("ApplicationName already exists."));

            CreateAuthenticationRequest request = new CreateAuthenticationRequest
            {
                ApplicationName = "Valid app",
            };

            ICreateAuthentication operation = new CreateAuthentication(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.InternalServerError);
            response.Success.Should().BeFalse();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Errors.Any(x => x.Code == OperationErrorCode.InternalError).Should().BeTrue();
            response.Data.Should().BeNull();
        }
    }
}
