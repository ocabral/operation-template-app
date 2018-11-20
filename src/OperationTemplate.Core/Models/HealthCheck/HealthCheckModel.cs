using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck
{
    /// <summary>
    /// Health Check Model.
    /// </summary>
    public class HealthCheckModel
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
        /// Application Version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Components.
        /// </summary>
        public IList<ApplicationComponentInfo> Components { get; set; }

        #region Mapping

        internal static HealthCheckResponse MapToResponse(HealthCheckModel model)
        {
            if (model == null)
            {
                return null;
            }

            HealthCheckResponse response = new HealthCheckResponse()
            {
                ApplicationName = model.ApplicationName,
                ApplicationType = (DataContracts.V1.HealthCheck.ApplicationType)model.ApplicationType,
                BuildDate = model.BuildDate,
                Components = ApplicationComponentInfo.MapToResponse(model.Components),
            };

            response.SetSuccessOk();

            return response;
        }

        internal static IList<HealthCheckResponse> MapToResponse(IList<HealthCheckModel> models)
        {
            return models?.Select(MapToResponse).ToList();
        }

        #endregion
    }
}

