using System;
using System.Threading.Tasks;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <inheritdoc />
    public class UpdateAuthenticationActivation : OperationBase<UpdateAuthenticationActivationRequest, UpdateAuthenticationActivationResponse>, IUpdateAuthenticationActivation
    {
        private IAuthenticationRepository _authenticationRepository = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationRepository"></param>
        public UpdateAuthenticationActivation(ILogger logger, IAuthenticationRepository authenticationRepository) : base(logger)
        {
            this._authenticationRepository = authenticationRepository;
        }

        /// <inheritdoc />
        protected override async Task<UpdateAuthenticationActivationResponse> ProcessOperationAsync(UpdateAuthenticationActivationRequest request)
        {
            using (this.Logger.StartInfoTrace("Starting process for Authentication Update."))
            {
                try
                {
                    int numberOfAffectedRows = await this._authenticationRepository.UpdateActivation(request.ApplicationKey, request.IsActive).ConfigureAwait(false);

                    var response = new UpdateAuthenticationActivationResponse();

                    if (numberOfAffectedRows == 1)
                    {
                        response.ApplicationKey = request.ApplicationKey;
                        response.IsActive = request.IsActive;

                        response.SetSuccessOk();
                    }
                    else
                    {
                        response.AddError(new OperationError("xxx", "Requested resource not found."), System.Net.HttpStatusCode.NotFound);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    this.Logger.Error("Error on UpdateAuthenticationActivation.", ex);

                    var responseError = new UpdateAuthenticationActivationResponse();
                    responseError.SetInternalServerError();

                    return responseError;
                }
            }
        }

        /// <inheritdoc />
        protected override async Task<UpdateAuthenticationActivationResponse> ValidateOperationAsync(UpdateAuthenticationActivationRequest request)
        {
            return await Task.Run(() =>
            {
                UpdateAuthenticationActivationResponse response = new UpdateAuthenticationActivationResponse();
                response.SetSuccessOk();

                if (request == null)
                {
                    response.AddError(new OperationError("xxx", "Request can not be null."));
                }

                if (string.IsNullOrWhiteSpace(request.ApplicationKey))
                {
                    response.AddError(new OperationError("xxx", "ApplicationKey can not be null."));
                }

                return response;
            });
        }
    }
}
