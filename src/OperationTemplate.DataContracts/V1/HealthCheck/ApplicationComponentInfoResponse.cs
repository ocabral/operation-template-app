namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck
{
    /// <summary>
    /// Object that contains the return of the Application Component as well as its data.
    /// See more information at: <see cref="ApplicationComponentInfoResponse"/>.
    /// </summary>
    public class ApplicationComponentInfoResponse : ApplicationInfoResponse
    {
        /// <summary>
        /// Additional Data.
        /// </summary>
        public string AdditionalData { get; set; }
    }
}
