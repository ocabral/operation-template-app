namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck
{
    public class GetApplicationInfoResponse : ApplicationInfoResponse
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public GetApplicationInfoResponse() { }

        /// <summary>
        /// Builds an ApplicationInfoResponse.
        /// </summary>
        /// <param name="applicationInfoResponse"></param>
        public GetApplicationInfoResponse(ApplicationInfoResponse applicationInfoResponse)
        {
            if (applicationInfoResponse != null)
            {
                base.ApplicationName = applicationInfoResponse.ApplicationName;
                base.ApplicationType = applicationInfoResponse.ApplicationType;
                base.BuildDate = applicationInfoResponse.BuildDate;
                base.Components = applicationInfoResponse.Components;
                base.MachineName = applicationInfoResponse.MachineName;
                base.OS = applicationInfoResponse.OS;
                base.Status = applicationInfoResponse.Status;
                base.Timestamp = applicationInfoResponse.Timestamp;
                base.Version = applicationInfoResponse.Version;
            }
        }
    }
}
