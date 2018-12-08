using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <inheritdoc />
    public class ValidateAuthentication : OperationBase<ValidateAuthenticationRequest, ValidateAuthenticationResponse>, IValidateAuthentication
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IAuthenticationMemoryCache _authenticationCache;
        private readonly int _authorizationTimeoutExpirationInSeconds;

        public ValidateAuthentication(ILogger logger, IAuthenticationRepository authenticationRepository, IAuthenticationMemoryCache authenticationCache, int authorizationTimeoutExpirationInSeconds = 30)
            : base(logger)
        {
            this._authenticationRepository = authenticationRepository;
            this._authenticationCache = authenticationCache;
            this._authorizationTimeoutExpirationInSeconds = authorizationTimeoutExpirationInSeconds;
        }

        /// <inheritdoc />
        protected override async Task<ValidateAuthenticationResponse> ProcessOperationAsync(ValidateAuthenticationRequest request)
        {
            ValidateAuthenticationResponse response = new ValidateAuthenticationResponse();

            int firstSplit = request.HeaderAuthorizationContent.IndexOf(":");
            string applicationKey = request.HeaderAuthorizationContent.Substring(0, firstSplit);
            int secondSplit = request.HeaderAuthorizationContent.IndexOf(":", firstSplit + 1);
            string clientHash = request.HeaderAuthorizationContent.Substring(firstSplit + 1, secondSplit - (firstSplit + 1));
            string clientTimeStamp = request.HeaderAuthorizationContent.Substring(secondSplit + 1, request.HeaderAuthorizationContent.Length - (secondSplit + 1));

            DateTime clientTokenDateTime = DateTime.Parse(clientTimeStamp).ToUniversalTime();

            DateTime futureValidDateTime = DateTime.UtcNow.AddSeconds(this._authorizationTimeoutExpirationInSeconds);
            if (clientTokenDateTime > futureValidDateTime)
            {
                response.SetUnauthorizedError();
                return response;
            }

            DateTime lastValidDateTime = DateTime.UtcNow.AddSeconds(-this._authorizationTimeoutExpirationInSeconds);
            if (lastValidDateTime >= clientTokenDateTime)
            {
                response.SetUnauthorizedError();
                return response;
            }

            AuthenticationModel authentication = await this._authenticationCache
                .GetItemAsync(applicationKey, this.GetAuthenticationAsync)
                .ConfigureAwait(false);

            if (authentication == null || authentication.IsActive == false)
            {
                response.SetUnauthorizedError();
                return response;
            }

            byte[] calculatedServerHashByteArray = authentication.ApplicationToken
                .GetHMACSHA256ByteArray((authentication.ApplicationName + clientTimeStamp)
                .ToUpper());

            string calculatedServerHash = Convert.ToBase64String(calculatedServerHashByteArray);

            bool isValid = (authentication.ApplicationKey == applicationKey) && (clientHash == calculatedServerHash);

            if (isValid)
            {
                response.IsValid = true;
                response.ApplicationName = authentication.ApplicationName;

                response.SetSuccessOk();
            }
            else
            {
                response.SetUnauthorizedError();
            }

            return response;
        }

        /// <inheritdoc />
        protected override async Task<ValidateAuthenticationResponse> ValidateOperationAsync(ValidateAuthenticationRequest request)
        {
            return await Task.Run(() =>
            {
                ValidateAuthenticationResponse response = new ValidateAuthenticationResponse();
                response.SetSuccessOk();

                if (request == null || string.IsNullOrWhiteSpace(request.HeaderAuthorizationContent))
                {
                    response.AddError(new OperationError(OperationErrorCode.RequestValidationError, "Request can not be null"));
                }

                return response;
            });
        }

        /// <summary>
        /// Get authentication asynchronously.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        private async Task<AuthenticationModel> GetAuthenticationAsync(string applicationKey)
        {
            return (await this._authenticationRepository.GetByFilter(
                new GetAuthenticationsRequest
                {
                    IsActive = true,
                    ApplicationKey = applicationKey
                })
                .ConfigureAwait(false))
                .FirstOrDefault();
        }
    }
}
