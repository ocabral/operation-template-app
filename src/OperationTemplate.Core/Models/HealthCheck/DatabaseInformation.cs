using System;

namespace StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck
{
    /// <summary>
    /// Database Information Model.
    /// </summary>
    public class DatabaseInformation
    {
        /// <summary>
        /// Last database modification date. Retrieves last modification on datbase schema.
        /// </summary>
        public DateTime LastDatabaseModificationDate { get; set; }

        /// <summary>
        /// Host name. Retrieves hostName and instance.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Version. Retrieves database name and version and the OS name and version where the database is installed.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Current database DateTime.
        /// </summary>
        public DateTime CurrentDateTime { get; set; }
    }
}
