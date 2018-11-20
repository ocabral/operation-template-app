namespace StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck
{
    /// <summary>
    /// Application Type for health Check.
    /// </summary>
    public enum ApplicationType
    {
        /// <summary>
        /// Web Service.
        /// </summary>
        WebService = 10,

        /// <summary>
        /// Queue Service.
        /// </summary>
        QueueService = 20,

        /// <summary>
        /// Database.
        /// </summary>
        Database = 30,

        /// <summary>
        /// Any other type.
        /// </summary>
        Other = 99,
    }
}
