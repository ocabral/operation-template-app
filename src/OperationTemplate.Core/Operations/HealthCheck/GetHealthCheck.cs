using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.HealthCheck
{
    /// <summary>
    /// Operation responsible for check the application health and yours dependencies to work successful.
    /// </summary>
    public class GetHealthCheck : OperationBase<GetHealthCheckRequest, GetHealthCheckResponse>, IGetHealthCheck
    {
        private DataContracts.V1.HealthCheck.ApplicationType? _applicationType = null;
        private IList<IUnitOfWork> _unitOfWorkList = null;
        private IList<IRepository> _repositoryList = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="applicationType"></param>
        public GetHealthCheck(ILogger logger, IUnitOfWork unitOfWork, DataContracts.V1.HealthCheck.ApplicationType? applicationType = null) : base(logger, null, unitOfWork)
        {
            this._applicationType = applicationType;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWorks"></param>
        /// <param name="applicationType"></param>
        public GetHealthCheck(ILogger logger, IList<IUnitOfWork> unitOfWorks, DataContracts.V1.HealthCheck.ApplicationType? applicationType = null) : base(logger, null, null)
        {
            this._applicationType = applicationType;
            this._unitOfWorkList = unitOfWorks;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repositories"></param>
        /// <param name="applicationType"></param>
        public GetHealthCheck(ILogger logger, IList<IRepository> repositories, DataContracts.V1.HealthCheck.ApplicationType? applicationType = null) : base(logger, null, null)
        {
            this._applicationType = applicationType;
            this._repositoryList = repositories;
        }

        /// <inheritdoc />
        protected override async Task<GetHealthCheckResponse> ProcessOperationAsync(GetHealthCheckRequest request)
        {
            HealthCheckModel healthCheck = new HealthCheckModel();
            healthCheck.ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
            healthCheck.ApplicationType = this._applicationType != null ? ((Models.HealthCheck.ApplicationType)this._applicationType) : Models.HealthCheck.ApplicationType.WebService; // Web Service as default value.
            healthCheck.BuildDate = File.GetLastWriteTime(Assembly.GetEntryAssembly().Location);

            healthCheck.Components = new List<ApplicationComponentInfo>();

            if(this._unitOfWorkList != null)
            {
                foreach (var uow in this._unitOfWorkList)
                {
                    healthCheck.Components.Add(await uow
                    .GetDatabaseInfo()
                    .ConfigureAwait(false));
                }
            }

            if (this._repositoryList != null)
            {
                foreach (var repository in this._repositoryList)
                {
                    healthCheck.Components.Add(await repository
                    .GetDatabaseInfo()
                    .ConfigureAwait(false));
                }
            }

            GetHealthCheckResponse response = new GetHealthCheckResponse(HealthCheckModel.MapToResponse(healthCheck));

            foreach (var component in healthCheck.Components)
            {
                if (component.Status != Models.HealthCheck.ApplicationStatus.Ok)
                {
                    response.AddError(new OperationError("", $"Component '{component.ApplicationName}' is {component.Status}."), HttpStatusCode.ServiceUnavailable);
                }
            }

            if (response.Errors == null || !response.Errors.Any())
            {
                response.SetSuccessOk();
            }

            return response;
        }
    }
}
