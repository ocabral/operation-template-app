using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System.Collections.Generic;
using System.Linq;

namespace StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck
{
    /// <summary>
    /// Application Component Info.
    /// </summary>
    public class ApplicationComponentInfo : ApplicationInfo
    {
        /// <summary>
        /// Additional Data.
        /// </summary>
        public string AdditionalData { get; set; }

        #region Mapping

        internal static ApplicationComponentInfoResponse MapToResponse(ApplicationComponentInfo model)
        {
            if (model == null)
            {
                return null;
            }

            ApplicationComponentInfoResponse response = new ApplicationComponentInfoResponse();
            response = ApplicationInfo.MapToApplicationComponentInfoResponse(model);
            response.AdditionalData = model.AdditionalData;

            return response;
        }

        internal static IList<ApplicationComponentInfoResponse> MapToResponse(IList<ApplicationComponentInfo> applicationComponentsInfo)
        {
            return applicationComponentsInfo?.Select(MapToResponse).ToList();
        }

        #endregion
    }
}
