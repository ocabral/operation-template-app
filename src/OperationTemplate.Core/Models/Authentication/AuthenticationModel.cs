using System;
using System.Collections.Generic;
using System.Linq;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Models.Authentication
{
    /// <summary>
    /// Represents the Authentication entity.
    /// </summary>
    public class AuthenticationModel
    {
        /// <summary>
        /// Internal identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Application name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Application key.
        /// All applications must inform the application key to authenticate.
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// Application Token. 
        /// It is used to generate the hash of authentication.
        /// </summary>
        public string ApplicationToken { get; set; }

        /// <summary>
        /// Indicate if the authentication is active / enable.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Creation DateTime.
        /// </summary>
        public DateTimeOffset CreationDateTime { get; set; }

        #region Mapping

        internal static AuthenticationResponse MapToResponse(AuthenticationModel authentication)
        {
            if(authentication == null)
            {
                return null;
            }

            return new AuthenticationResponse()
            {
                ApplicationKey = authentication.ApplicationKey,
                ApplicationName = authentication.ApplicationName,
                ApplicationToken = authentication.ApplicationToken,
                IsActive = authentication.IsActive
            };
        }

        internal static IList<AuthenticationResponse> MapToResponse(IList<AuthenticationModel> authentications)
        {
            if (authentications == null || authentications.Count == 0)
            {
                return null;
            }

            return authentications.Select(MapToResponse).ToList();
        }

        #endregion
    }
}
