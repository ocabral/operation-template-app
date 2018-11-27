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

        internal static ApplicationComponentInfoResponse MapToApplicationComponentInfoResponse(ApplicationInfo model)
        {
            if (model == null)
            {
                return null;
            }

            ApplicationComponentInfoResponse response = new ApplicationComponentInfoResponse();

            response.AdditionalData = null;
            response.ApplicationName = model.ApplicationName;
            response.ApplicationType = (DataContracts.V1.HealthCheck.ApplicationType)model.ApplicationType;
            response.BuildDate = model.BuildDate;
            response.MachineName = model.MachineName;
            //Components = model.Components,

            if (model.OS != null)
            {
                response.OS = new DataContracts.V1.HealthCheck.OS()
                {
                    Name = model.OS.Name,
                    Version = model.OS.Version,
                };
            }

            response.Status = (DataContracts.V1.HealthCheck.ApplicationStatus)model.Status;
            response.Timestamp = model.Timestamp;
            response.Version = model.Version;

            response.SetSuccessOk();

            return response;
        }

        internal static ApplicationInfoResponse MapToResponse(ApplicationInfo model)
        {
            if (model == null)
            {
                return null;
            }

            ApplicationInfoResponse response = new ApplicationInfoResponse();

            response.ApplicationName = model.ApplicationName;
            response.ApplicationType = (DataContracts.V1.HealthCheck.ApplicationType)model.ApplicationType;
            response.BuildDate = model.BuildDate;
            response.MachineName = model.MachineName;
            //Components = model.Components,

            if (model.OS != null)
            {
                response.OS = new DataContracts.V1.HealthCheck.OS()
                {
                    Name = model.OS.Name,
                    Version = model.OS.Version,
                };
            }

            response.Status = (DataContracts.V1.HealthCheck.ApplicationStatus)model.Status;
            response.Timestamp = model.Timestamp;
            response.Version = model.Version;

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
