using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    public interface IRepository
    {
        /// <summary>
        /// Settings related to database.
        /// </summary>
        IDatabaseSettings DatabaseSettings { get; }

        /// <summary>
        /// Instance of logger. See <see cref="ILogger"/>.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Database connection object.
        /// </summary>
        IDbConnector DbConnector { get; }

        /// <summary>
        /// Get some database information.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        Task<ApplicationComponentInfo> GetDatabaseInfo();
    }
}
