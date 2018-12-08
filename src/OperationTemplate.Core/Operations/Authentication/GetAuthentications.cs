using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <inheritdoc />
    public class GetAuthentications : OperationBase<GetAuthenticationsRequest, GetAuthenticationsResponse>, IGetAuthentications
    {
        private IAuthenticationRepository _authenticationRepository = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationRepository"></param>
        public GetAuthentications(ILogger logger, IAuthenticationRepository authenticationRepository) : base(logger)
        {
            this._authenticationRepository = authenticationRepository;
        }

        /// <inheritdoc />
        protected override async Task<GetAuthenticationsResponse> ProcessOperationAsync(GetAuthenticationsRequest request)
        {
            using (this.Logger.StartInfoTrace("Starting process for Authentication Get."))
            {
                GetAuthenticationsResponse response = new GetAuthenticationsResponse();
                response.SetSuccessOk();

                IList<AuthenticationModel> authenticationList = await this._authenticationRepository.GetByFilter(request).ConfigureAwait(false);

                if (authenticationList == null || authenticationList.Count == 0)
                {
                    response.AddError(new OperationError(OperationErrorCode.RequestValidationError, "Requested resource not found."), HttpStatusCode.NotFound);
                    return response;
                }

                response.Data = AuthenticationModel.MapToResponse(authenticationList);

                return response;
            }
        }

        /// <inheritdoc />
        protected override async Task<GetAuthenticationsResponse> ValidateOperationAsync(GetAuthenticationsRequest request)
        {
            return await Task.Run(() =>
            {
                GetAuthenticationsResponse response = new GetAuthenticationsResponse();
                response.SetSuccessOk();

                if (request == null)
                {
                    response.AddError(new OperationError(OperationErrorCode.RequestValidationError, "Request can not be null."));
                    return response;
                }

                return response;
            });
        }
    }
}
