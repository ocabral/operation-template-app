namespace StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck
{
    /// <summary>
    /// Application Status for Health check.
    /// </summary>
    public enum ApplicationStatus
    {
        /// <summary>
        /// Ok. Avaiable.
        /// </summary>
        Ok = 10,

        /// <summary>
        /// Partially Available.
        /// </summary>
        PartiallyAvailable = 20,

        /// <summary>
        /// Critical.
        /// </summary>
        Critical = 30,
    }
}
