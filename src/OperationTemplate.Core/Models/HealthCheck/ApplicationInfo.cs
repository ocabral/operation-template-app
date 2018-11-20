using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck
{
    /// <summary>
    /// Application Info Model.
    /// </summary>
    public class ApplicationInfo : HealthCheckModel
    {
        /// <summary>
        /// Machine Name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Operating System.
        /// </summary>
        public OS OS { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public ApplicationStatus Status { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        #region Mapping

        internal static ApplicationInfoResponse MapToResponse(ApplicationInfo model)
        {
            if (model == null)
            {
                return null;
            }

            ApplicationInfoResponse response = new ApplicationInfoResponse()
            {
                ApplicationName = model.ApplicationName,
                ApplicationType = (DataContracts.V1.HealthCheck.ApplicationType)model.ApplicationType,
                BuildDate = model.BuildDate,
                MachineName = model.MachineName,
                //Components = model.Components,
                OS = new DataContracts.V1.HealthCheck.OS()
                {
                    Name = model.OS.Name,
                    Version = model.OS.Version,
                },

                Status = (DataContracts.V1.HealthCheck.ApplicationStatus)model.Status,
                Timestamp = model.Timestamp,
                Version = model.Version,
            };

            response.SetSuccessOk();

            return response;
        }

        internal static IList<ApplicationInfoResponse> MapToResponse(IList<ApplicationInfo> applicationsInfo)
        {
            return applicationsInfo?.Select(MapToResponse).ToList();
        }

        #endregion
    }

    /// <summary>
    /// Operating System.
    /// </summary>
    public class OS
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Operating System Version.
        /// </summary>
        public string Version { get; set; }
    }
}
