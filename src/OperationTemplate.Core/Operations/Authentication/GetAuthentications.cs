using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
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

                IList<AuthenticationModel> authenticationList = await this._authenticationRepository.GetByFilter(request).ConfigureAwait(false);

                if (authenticationList == null || authenticationList.Count == 0)
                {
                    response.AddError(new OperationError("xxx", "Requested resource not found."), HttpStatusCode.NotFound);
                }

                response.Data = AuthenticationModel.MapToResponse(authenticationList);

                response.SetSuccessOk();

                return response;
            }
        }
    }
}
