using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using System;
using System.Data;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        /// <summary>
        /// Flag to identify if dispose has already been called.
        /// </summary>
        protected bool disposed;

        protected IDatabaseSettings DatabaseSettings { get; }

        protected ILogger Logger { get; }

        protected IDbConnection DbConnection { get; }

        public UnitOfWorkBase(IDatabaseSettings dbSettings, ILogger logger, IDbConnection dbConnection = null)
        {
            this.DatabaseSettings = dbSettings;
            this.Logger = logger;
            this.DbConnection = dbConnection;
        }

        public abstract void Dispose();
    }
}
