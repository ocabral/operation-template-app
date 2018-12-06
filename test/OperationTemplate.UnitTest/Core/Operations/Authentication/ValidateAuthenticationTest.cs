using FluentAssertions;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Model = StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Operations.Authentication
{
    public class ValidateAuthenticationTest
    {
        private IList<Model.AuthenticationModel> _authenticationList = new List<Model.AuthenticationModel>()
        {
            new Model.AuthenticationModel()
            {
                ApplicationKey = "Valid app key",
                ApplicationName = "App test",
                ApplicationToken = "Valid app token",
                CreationDateTime = DateTimeOffset.UtcNow,
                Id = 123,
                IsActive = true,
            }
        };

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessTrue_WhenRequestIsValid()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "App test";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = $"Valid app key:{clientHash}:{clientTimeStamp}",
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.Errors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessTrue_WhenRequestIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "App test";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = null;

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessTrue_WhenHeaderAuthorizationContentIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "App test";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = null,
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessFalse_WhenClientTimeStampIsExpired()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "App test";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.AddSeconds(-30).ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = $"Valid app key:{clientHash}:{clientTimeStamp}",
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessFalse_WhenClientTimeStampIsGeneratedInFuture()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "App test";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.AddSeconds(500000).ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = $"Valid app key:{clientHash}:{clientTimeStamp}",
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessFalse_WhenApplicationNameDoesNotMatch()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "Mismatch Application Name";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = $"Valid app key:{clientHash}:{clientTimeStamp}",
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessFalse_WhenClientHashIsInvalid()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.ToString();
            string clientHash = applicationToken.GetHMACSHA256(("Invalid" + "Hash").ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = $"Valid app key:{clientHash}:{clientTimeStamp}",
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void ValidateAuthentication_ShouldReturnSuccessFalse_WhenApplicationIsNotActive()
        {
            // Arrange
            this._authenticationList.FirstOrDefault().IsActive = false;

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                    .Returns(() => Task.FromResult(this._authenticationList));

            IAuthenticationMemoryCache memoryCache = new AuthenticationMemoryCache();

            string applicationName = "App test";
            string applicationToken = "Valid app token";
            string clientTimeStamp = DateTimeOffset.UtcNow.ToString();
            string clientHash = applicationToken.GetHMACSHA256((applicationName + clientTimeStamp).ToUpper());

            ValidateAuthenticationRequest request = new ValidateAuthenticationRequest()
            {
                HeaderAuthorizationContent = $"Valid app key:{clientHash}:{clientTimeStamp}",
            };

            IValidateAuthentication operation = new ValidateAuthentication(loggerMock.Object, repositoryMock.Object, memoryCache);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            response.Success.Should().BeFalse();
            response.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}
