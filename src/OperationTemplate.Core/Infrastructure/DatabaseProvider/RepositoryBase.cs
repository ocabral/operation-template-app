using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Repository with common implementations between others repositories.
    /// </summary>
    public abstract class RepositoryBase
    {
        protected IDatabaseSettings DatabaseSettings { get; }

        protected ILogger Logger { get; }

        protected IDbConnector DbConnector { get; }

        public RepositoryBase(IDatabaseSettings dbSettings, ILogger logger, IDbConnector dbConnector)
        {
            this.DatabaseSettings = dbSettings;
            this.Logger = logger;
            this.DbConnector = dbConnector;
        }
    }
}
