using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using System.Data;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Abstract class that must be inherited when implementing a concrete repository.
    /// </summary>
    public abstract class RepositoryBase
    {
        protected IDatabaseSettings DatabaseSettings { get; }

        public ILogger Logger { get; }

        protected IDbConnection DbConnection { get; }

        public RepositoryBase(IDatabaseSettings dbSettings, ILogger logger, IDbConnection dbConnection = null)
        {
            this.DatabaseSettings = dbSettings;
            this.Logger = logger;
            this.DbConnection = dbConnection;
        }
    }
}
