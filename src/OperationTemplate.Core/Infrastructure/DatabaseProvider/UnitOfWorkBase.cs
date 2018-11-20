using Dapper;
using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Sql;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        /// Flag to identify if dispose has already been called.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Settings related to database.
        /// </summary>
        protected IDatabaseSettings DatabaseSettings { get; }

        /// <summary>
        /// Instance of log.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Database connection object.
        /// </summary>
        private IDbConnector DbConnector { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class.
        /// </summary>
        /// <param name="dbSettings">Some database Settings. See ref <see cref="IDatabaseSettings"/></param>
        /// <param name="logger">The logger instance. See ref <see cref="ILogger"/></param>
        /// <param name="dbConnector">Database connector. See ref <see cref="IDbConnector"/>.</param>
        public UnitOfWorkBase(IDatabaseSettings dbSettings, ILogger logger, IDbConnector dbConnector)
        {
            this.DatabaseSettings = dbSettings;
            this.Logger = logger;
            this.DbConnector = dbConnector;
        }

        #region Main methods

        /// <summary>
        /// Initiates a database transaction under the connection held for <see cref="IUnitOfWork"/> instance.
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                this.DbConnector.BeginTransaction();
            }
            catch (Exception e)
            {
                this.Logger.DatabaseError(e);
                throw;
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                this.DbConnector.CommitTransaction();
            }
            catch (Exception e)
            {
                this.Logger.DatabaseError(e);
                throw;
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                this.DbConnector.RollbackTransaction();
            }
            catch (Exception e)
            {
                this.Logger.DatabaseError(e);
                throw;
            }
        }

        /// <summary>
        /// Get Database information for Health check.
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationComponentInfo> GetDatabaseInfo()
        {
            try
            {
                // Execute Sql query.
                string sqlTemplate = OperationTemplateSqlResource.GetDatabaseInformation;

                IEnumerable<DatabaseInformation> queryResult = await this.DbConnector.Connection
                    .QueryAsync<DatabaseInformation>(sqlTemplate)
                    .ConfigureAwait(false);

                DatabaseInformation result = queryResult.FirstOrDefault();

                ApplicationComponentInfo databaseInfoModel = new ApplicationComponentInfo
                {
                    AdditionalData = string.Empty,
                    ApplicationName = this.DbConnector.Connection.Database,
                    ApplicationType = ApplicationType.Database,
                    BuildDate = result.LastDatabaseModificationDate,
                    MachineName = result.HostName.Trim()
                };

                // Get OS Name and Version.
                int index = result.Version.IndexOf(" on ", StringComparison.InvariantCultureIgnoreCase);
                index += 4;
                int pos = result.Version.IndexOf(" <X64>", index, StringComparison.InvariantCultureIgnoreCase);

                string osNameAndVersion = result.Version.Substring(index, pos - index);

                int posEndOfOsName = osNameAndVersion.LastIndexOf(" ");
                string osName = result.Version.Substring(index, result.Version.Length - index - 1).Trim();
                string osVersion = osNameAndVersion.Substring(posEndOfOsName, osNameAndVersion.Length - posEndOfOsName).Trim();

                databaseInfoModel.OS = new OS { Name = osName, Version = osVersion };
                databaseInfoModel.Status = ApplicationStatus.Ok;
                databaseInfoModel.Timestamp = result.CurrentDateTime;

                // Get Database Version.
                pos = result.Version.IndexOf("(X64)", StringComparison.InvariantCultureIgnoreCase);

                if (pos == 0)
                {
                    pos = result.Version.LastIndexOf("(X86)", StringComparison.InvariantCultureIgnoreCase);
                }
                pos += 5;

                databaseInfoModel.Version = result.Version.Substring(0, pos).Trim();

                return databaseInfoModel;
            }
            catch (Exception ex)
            {
                this.Logger.DatabaseError(ex);
                throw;
            }
        }

        #endregion

        #region Dispose implementation.

        /// <inheritdoc />
        /// <summary>
        /// Dispose the resources used along the lifecycle of this instance.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed) { return; }

            this.DbConnector?.Dispose();
            GC.SuppressFinalize(this);

            this._disposed = true;
        }

        #endregion
    }
}
