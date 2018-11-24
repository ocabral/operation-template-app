using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Repository with common implementations between others repositories.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Settings related to database.
        /// </summary>
        protected IDatabaseSettings DatabaseSettings { get; }

        /// <summary>
        /// Instance of logger. See <see cref="ILogger"/>.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Database connection object.
        /// </summary>
        protected IDbConnector DbConnector { get; }

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
