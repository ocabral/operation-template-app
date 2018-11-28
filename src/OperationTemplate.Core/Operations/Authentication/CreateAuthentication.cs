using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <inheritdoc />
    public class CreateAuthentication : OperationBase<CreateAuthenticationRequest, CreateAuthenticationResponse>, ICreateAuthentication
    {
        private IAuthenticationRepository _authenticationRepository = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public CreateAuthentication(ILogger logger, IAuthenticationRepository authenticationRepository) : base(logger, null, null)
        {
            this._authenticationRepository = authenticationRepository;
        }

        /// <inheritdoc />
        protected override async Task<CreateAuthenticationResponse> ProcessOperationAsync(CreateAuthenticationRequest request)
        {
            using (this.Logger.StartInfoTrace("Starting process for Authentication Insert."))
            {
                AuthenticationModel authentication = new AuthenticationModel()
                {
                    ApplicationKey = Guid.NewGuid().ToString("N"),
                    ApplicationName = request.ApplicationName,
                    ApplicationToken = CryptographyExtensions.GenerateRandomClientToken(),
                    CreationDateTime = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                await this._authenticationRepository.Insert(authentication);

                var response = new CreateAuthenticationResponse()
                {
                    Data = AuthenticationModel.MapToResponse(authentication),
                };

                response.SetSuccessOk();

                return response;
            }
        }

        /// <inheritdoc />
        protected override async Task<CreateAuthenticationResponse> ValidateOperationAsync(CreateAuthenticationRequest request)
        {
            return await Task.Run(() =>
            {
                CreateAuthenticationResponse response = new CreateAuthenticationResponse();
                response.SetSuccessOk();

                if (request == null)
                {
                    response.AddError(new OperationError("xxx", "Request can not be null."));
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.ApplicationName))
                {
                    response.AddError(new OperationError("xxx", "ApplicationName can not be null."));
                }

                return response;
            });
        }
    }
}
