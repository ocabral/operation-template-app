using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <inheritdoc />
    public class GetAuthentication : OperationBase<GetAuthenticationsRequest, GetAuthenticationResponse>, IGetAuthentication
    {
        private IAuthenticationRepository _authenticationRepository = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationRepository"></param>
        public GetAuthentication(ILogger logger, IAuthenticationRepository authenticationRepository) : base(logger)
        {
            this._authenticationRepository = authenticationRepository;
        }

        /// <inheritdoc />
        protected override async Task<GetAuthenticationResponse> ProcessOperationAsync(GetAuthenticationsRequest request)
        {
            using (this.Logger.StartInfoTrace("Starting process for Authentication Get."))
            {
                try
                {
                    GetAuthenticationResponse response = new GetAuthenticationResponse();

                    IList<AuthenticationModel> authenticationList = await this._authenticationRepository.GetByFilter(request).ConfigureAwait(false);

                    if (authenticationList == null || authenticationList.Count == 0)
                    {
                        response.AddError(new OperationError("xxx", "Requested resource not found."), HttpStatusCode.NotFound);
                    }

                    response.Data = AuthenticationModel.MapToResponse(authenticationList.First());

                    response.SetSuccessOk();

                    return response;
                }
                catch (Exception ex)
                {
                    this.Logger.Error("Error on GetAuthentications.", ex);

                    var responseError = new GetAuthenticationResponse();
                    responseError.SetInternalServerError();

                    return responseError;
                }
            }
        }

        /// <inheritdoc />
        protected override async Task<GetAuthenticationResponse> ValidateOperationAsync(GetAuthenticationsRequest request)
        {
            return await Task.Run(() =>
            {
                GetAuthenticationResponse response = new GetAuthenticationResponse();

                if (string.IsNullOrWhiteSpace(request.ApplicationKey))
                {
                    response.AddError(new OperationError("xxx", "ApplicationKey can not be null."));
                }

                return response;
            });
        }
    }
}
