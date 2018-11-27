using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.HealthCheck
{
    /// <summary>
    /// Operation responsible for return the application information as name, version, type, build date and etc.
    /// </summary>
    public class GetApplicationInfo : OperationBase<GetApplicationInfoRequest, GetApplicationInfoResponse>, IGetApplicationInfo
    {
        private DataContracts.V1.HealthCheck.ApplicationType? _applicationType = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationType"></param>
        public GetApplicationInfo(ILogger logger, DataContracts.V1.HealthCheck.ApplicationType? applicationType = null) : base(logger)
        {
            this._applicationType = applicationType;
        }

        /// <inheritdoc />
        protected override async Task<GetApplicationInfoResponse> ProcessOperationAsync(GetApplicationInfoRequest request)
        {
            ApplicationInfo applicationInfo = new ApplicationInfo
            {
                ApplicationName = Assembly.GetEntryAssembly().GetName().Name,
                ApplicationType = this._applicationType != null ? ((Models.HealthCheck.ApplicationType)this._applicationType) : Models.HealthCheck.ApplicationType.WebService, // Web Service as default value.
                BuildDate = File.GetLastWriteTime(Assembly.GetEntryAssembly().Location),
                MachineName = Environment.MachineName,
                OS = new Models.HealthCheck.OS { Name = RuntimeInformation.OSDescription.Trim(), Version = Environment.OSVersion.Version.ToString() },
                Status = Models.HealthCheck.ApplicationStatus.Ok,
                Timestamp = DateTime.Now,
                Version = Assembly.GetEntryAssembly().GetName().Version.ToString(),
            };

            var result = new GetApplicationInfoResponse(ApplicationInfo.MapToResponse(applicationInfo));
            result.SetSuccessOk();

            return await Task.FromResult(result);
        }
    }
}
