using System;
using System.Collections.Generic;

namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck
{
    public class GetHealthCheckResponse : HealthCheckResponse
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public GetHealthCheckResponse() { }

        /// <summary>
        /// Builds a HealthCheckResponse
        /// </summary>
        /// <param name="healthCheckResponse"></param>
        public GetHealthCheckResponse(HealthCheckResponse healthCheckResponse)
        {
            if (healthCheckResponse != null)
            {
                base.ApplicationName = healthCheckResponse.ApplicationName;
                base.ApplicationType = healthCheckResponse.ApplicationType;
                base.BuildDate = healthCheckResponse.BuildDate;
                base.Components = healthCheckResponse.Components;
            }
        }
    }
}
