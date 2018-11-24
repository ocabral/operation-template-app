#if NET471
using StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace StoneCo.Buy4.OperationTemplate.WebApi.Middlewares.ActionFilters.NetFramework
{
    /// <summary>
    /// Web Api Authentication Request Middleware.
    /// </summary>
    public class AuthenticationRequestMiddleware : ActionFilterAttribute
    {
        private readonly IValidateAuthentication _validateAuthentication;

        /// <summary>
        /// Authentication Request Middleware Constructor. This middleware intercept requests to authenticate the request origin token
        /// </summary>
        public AuthenticationRequestMiddleware(IValidateAuthentication validateAuthentication)
        {
            this._validateAuthentication = validateAuthentication;
        }

        /// <summary>
        /// Intercepts all requests to validate Authentication Token.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            string absolutePath = actionContext.Request.RequestUri.AbsolutePath;

            // We use case-insensitive routes
            if (!absolutePath.StartsWith("/api", StringComparison.InvariantCultureIgnoreCase)
                || absolutePath.IndexOf("authentication", StringComparison.InvariantCultureIgnoreCase) >= 0
                || absolutePath.IndexOf("management", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                await base.OnActionExecutingAsync(actionContext, cancellationToken).ConfigureAwait(false);
                return;
            }

            HttpRequestHeaders headers = actionContext.Request.Headers;
            const string authorizationKey = "Authorization";
            IEnumerable<string> fullAuthorizationHeaderContent = headers.Contains(authorizationKey) ? headers.GetValues(authorizationKey) : null;

            // Get authentication token from cache.
            // If cache does not have the token this will be searched in database using GetAuthenticationClientApiKeyAsync function
            // and stored in cache for future use.
            ValidateAuthenticationResponse response = null;

            if (fullAuthorizationHeaderContent?.Any() == true)
            {
                response = await this._validateAuthentication
                    .ProcessAsync(new ValidateAuthenticationRequest()
                    {
                        HeaderAuthorizationContent = fullAuthorizationHeaderContent.First()
                    })
                    .ConfigureAwait(false);
            }

            if (response != null && response.IsValid)
            {
                //actionContext.Request.GetOwinEnvironment().Add("ApplicationName", response.ApplicationName);
                await base.OnActionExecutingAsync(actionContext, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                if(response == null)
                {
                    response = new ValidateAuthenticationResponse();
                }

                response.SetUnauthorizedError();

                actionContext.Response = HttpResponseBuilder.BuildHttpResponse(response);
            }
        }
    }
}
#endif