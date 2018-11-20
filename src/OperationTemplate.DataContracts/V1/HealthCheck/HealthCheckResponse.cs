using System;
using System.Collections.Generic;

namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck
{
    /// <summary>
    /// Health Check Response.
    /// </summary>
    public class HealthCheckResponse : OperationResponseBase
    {
        /// <summary>
        /// Application Name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Application Type.
        /// </summary>
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// Build Date.
        /// </summary>
        public DateTime BuildDate { get; set; }

        /// <summary>
        /// Components.
        /// </summary>
        public IList<ApplicationComponentInfoResponse> Components { get; set; }
    }
}
