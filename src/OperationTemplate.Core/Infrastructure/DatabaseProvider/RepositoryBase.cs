using Dapper;
using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Sql;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.Core.Models.HealthCheck;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Repository with common implementations between others repositories.
    /// </summary>
    public abstract class RepositoryBase : IRepository
    {
        /// <summary>
        /// Settings related to database.
        /// </summary>
        public IDatabaseSettings DatabaseSettings { get; }

        /// <summary>
        /// Instance of logger. See <see cref="ILogger"/>.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Database connection object.
        /// </summary>
        public IDbConnector DbConnector { get; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dbSettings"></param>
        /// <param name="logger"></param>
        public RepositoryBase(IDatabaseSettings dbSettings, ILogger logger)
        {
            this.DatabaseSettings = dbSettings;
            this.Logger = logger;
            this.DbConnector = null;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dbSettings"></param>
        /// <param name="logger"></param>
        /// <param name="dbConnector"></param>
        public RepositoryBase(IDatabaseSettings dbSettings, ILogger logger, IDbConnector dbConnector)
        {
            this.DatabaseSettings = dbSettings;
            this.Logger = logger;
            this.DbConnector = dbConnector;
        }

        /// <summary>
        /// Get Database information for Health check.
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationComponentInfo> GetDatabaseInfo()
        {
            try
            {
                using (var connection = new SqlConnection(this.DatabaseSettings.ConnectionString))
                {
                    // Execute Sql query.
                    string sqlTemplate = OperationTemplateSqlResource.GetDatabaseInformation;

                    IEnumerable<DatabaseInformation> queryResult = await connection
                        .QueryAsync<DatabaseInformation>(sqlTemplate)
                        .ConfigureAwait(false);

                    DatabaseInformation result = queryResult.FirstOrDefault();

                    ApplicationComponentInfo databaseInfoModel = new ApplicationComponentInfo
                    {
                        AdditionalData = string.Empty,
                        ApplicationName = connection.Database,
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
            }
            catch (Exception ex)
            {
                this.Logger.DatabaseError(ex);
                throw;
            }
        }

        /// <summary>
        /// Build paging sql (rows limit and offset).
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected string BuildPagingSql(string sql, int limit, long offset = 0)
        {
            return sql.Replace("#PAGING", $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY");
        }
    }
}
