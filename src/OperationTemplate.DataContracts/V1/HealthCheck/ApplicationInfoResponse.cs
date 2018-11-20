using System;

namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.HealthCheck
{
    /// <summary>
    /// Object that contains the return of the Application Info as well as its data.
    /// See more information at: <see cref="ApplicationInfoResponse"/>.
    /// </summary>
    public class ApplicationInfoResponse : HealthCheckResponse
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

        /// <summary>
        /// API Version.
        /// </summary>
        public string Version { get; set; }
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
