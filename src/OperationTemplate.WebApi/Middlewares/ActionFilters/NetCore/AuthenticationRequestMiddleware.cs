#if NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.WebApi.Middlewares.ActionFilters.NetCore
{
    public class AuthenticationRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IValidateAuthentication _validateAuthentication;

        /// <summary>
        /// Authentication Request Middleware Constructor.
        /// </summary>
        public AuthenticationRequestMiddleware(RequestDelegate next, IValidateAuthentication validateAuthentication)
        {
            this._next = next;
            this._validateAuthentication = validateAuthentication;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            PathString pathString = httpContext.Request.Path;

            // We use case-insensitive routes
            if (!pathString.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase)
                || pathString.ToString().IndexOf("authentication", StringComparison.InvariantCultureIgnoreCase) >= 0
                || pathString.ToString().IndexOf("management", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                await this._next.Invoke(httpContext).ConfigureAwait(false);
                return;
            }

            IHeaderDictionary headers = httpContext.Request.Headers;
            const string authorizationKey = "Authorization";
            bool hasAuthorizationHeader = headers.TryGetValue(authorizationKey, out StringValues fullAuthorizationHeaderContent);

            // Get authentication token from cache.
            // If cache does not have the token this will be searched in database using GetAuthenticationTokenAsync function
            // and stored in cache for future use.
            ValidateAuthenticationResponse response = null;

            if (hasAuthorizationHeader)
            {
                response = await this._validateAuthentication
                    .ProcessAsync(new ValidateAuthenticationRequest()
                    {
                        HeaderAuthorizationContent = fullAuthorizationHeaderContent
                    })
                    .ConfigureAwait(false);
            }

            if (response != null && response.IsValid)
            {
                httpContext.Items.Add("ApplicationName", response.ApplicationName);
                await this._next.Invoke(httpContext).ConfigureAwait(false);
            }
            else
            {
                if (response == null)
                {
                    response = new ValidateAuthenticationResponse();
                }

                response.SetUnauthorizedError();

                HttpResponseMessage httpResponse = HttpResponseBuilder.BuildHttpResponse(response);

                httpContext.Response.StatusCode = (int)httpResponse.StatusCode;
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.Body = await httpResponse.Content.ReadAsStreamAsync();
            }
        }
    }
}
#endif