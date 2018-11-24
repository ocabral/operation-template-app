using System;
using System.Collections.Generic;
using System.Text;

namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication
{
    public class AuthenticationResponse
    {
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
    }
}
