using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System.Linq;
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
            // With the purpose to don't replicate code, this operation calls the GetAuthentications operation, because the code and the rules are the same on both.
            // The unique difference is the reponse. On GetAuthentication the reponse is AuthenticationResponse and on GetAuthentications the response is IList<AuthenticationResponse>.
            GetAuthenticationsResponse operationResponse = await new GetAuthentications(this.Logger, this._authenticationRepository)
                .ProcessAsync(request)
                .ConfigureAwait(false);

            var response = new GetAuthenticationResponse();

            if (operationResponse.Success)
            {
                response.Data = operationResponse.Data?.First();
                response.SetSuccessOk();
            }
            else
            {
                response.AddErrors(operationResponse.Errors);
                response.HttpStatusCode = operationResponse.HttpStatusCode;
            }

            return response;
        }

        /// <inheritdoc />
        protected override async Task<GetAuthenticationResponse> ValidateOperationAsync(GetAuthenticationsRequest request)
        {
            return await Task.Run(() =>
            {
                GetAuthenticationResponse response = new GetAuthenticationResponse();
                response.SetSuccessOk();

                if (request == null)
                {
                    response.AddError(new OperationError("xxx", "Request can not be null."));
                    return response;
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
