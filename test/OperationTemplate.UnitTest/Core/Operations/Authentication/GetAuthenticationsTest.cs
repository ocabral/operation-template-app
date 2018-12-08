using FluentAssertions;
using Moq;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Model = StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Operations.Authentication
{
    public class GetAuthenticationsTest
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
            },
            new Model.AuthenticationModel()
            {
                ApplicationKey = "Valid app2 key",
                ApplicationName = "App2 test",
                ApplicationToken = "Valid app2 token",
                CreationDateTime = DateTimeOffset.UtcNow,
                Id = 123,
                IsActive = true,
            },
            new Model.AuthenticationModel()
            {
                ApplicationKey = "Valid app3 key",
                ApplicationName = "App3 test",
                ApplicationToken = "Valid app3 token",
                CreationDateTime = DateTimeOffset.UtcNow,
                Id = 123,
                IsActive = true,
            },
        };

        [Fact]
        public async void GetAuthentication_ShouldReturnSuccessTrue_WhenRequestIsValid()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                .Returns(() => Task.FromResult(this._authenticationList));

            GetAuthenticationsRequest request = new GetAuthenticationsRequest
            {
                ApplicationKey = "Valid app key",
            };

            IGetAuthentications operation = new GetAuthentications(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Success.Should().BeTrue();
            response.Errors.Should().BeNullOrEmpty();
            response.Data.Should().HaveCountGreaterThan(0);
            response.Data.FirstOrDefault().ApplicationKey.Should().NotBeNull();
            response.Data.FirstOrDefault().ApplicationName.Should().NotBeNull();
            response.Data.FirstOrDefault().ApplicationToken.Should().NotBeNull();
            response.Data.FirstOrDefault().IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async void GetAuthentications_ShouldReturnSuccessFalse_WhenApplicationKeyDoesNotExist(bool emptList)
        {
            IList<Model.AuthenticationModel> authenticationList = null;

            if (emptList)
                authenticationList = new List<Model.AuthenticationModel>();

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                .Returns(() => Task.FromResult(authenticationList));

            GetAuthenticationsRequest request = new GetAuthenticationsRequest
            {
                ApplicationKey = "Inexistent app key",
            };

            IGetAuthentications operation = new GetAuthentications(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
            response.Success.Should().BeFalse();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.Data.Should().BeNull();
        }

        [Fact]
        public async void GetAuthentications_ShouldReturnSuccessFalse_WhenRequestIsNull()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IAuthenticationRepository> repositoryMock = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            repositoryMock
                .Setup(x => x.GetByFilter(It.IsAny<GetAuthenticationsRequest>()))
                .Returns(() => Task.FromResult(this._authenticationList));

            GetAuthenticationsRequest request = null;

            IGetAuthentications operation = new GetAuthentications(loggerMock.Object, repositoryMock.Object);

            // Act
            var response = await operation.ProcessAsync(request).ConfigureAwait(false);

            // Assert
            response.HttpStatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            response.Success.Should().BeFalse();
            response.Errors.Count().Should().BeGreaterThan(0);
            response.Errors.Any(x => x.Code == OperationErrorCode.RequestValidationError).Should().BeTrue();
            response.Data.Should().BeNull();
        }
    }
}
