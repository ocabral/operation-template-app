using StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;
using System;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Unit of Work interface that is responsible for managing the repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Initiates a database transaction under the connection held for <see cref="IUnitOfWork"/> instance.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Get some database information.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        Task<ApplicationComponentInfo> GetDatabaseInfo();
    }
}
