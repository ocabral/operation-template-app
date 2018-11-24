using System;

namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication
{
    /// <summary>
    /// Request to get application authentications according the informed fields.
    /// </summary>
    public class GetAuthenticationsRequest : OperationPagedRequestBase
    {
        /// <summary>
        /// Application Name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Authentication Token.
        /// </summary>
        public string ApplicationToken { get; set; }

        /// <summary>
        /// Authentication Key.
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// If the authentication is active.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Initial creation date (from the date entered).
        /// </summary>
        public DateTimeOffset? StartCreationDateTime { get; set; }

        /// <summary>
        /// Final creation date (up to the date entered).
        /// </summary>
        public DateTimeOffset? EndCreationDateTime { get; set; }
    }
}
