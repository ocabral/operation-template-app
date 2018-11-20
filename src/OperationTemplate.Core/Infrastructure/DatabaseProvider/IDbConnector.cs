using System;
using System.Data;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Responsible for handling a database connection, processing database transactions
    /// and ensuring the consistency of the transaction.
    /// </summary>
    public interface IDbConnector : IDisposable
    {
        /// <summary>
        /// Database connection, through which all transactions will be committed to.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Database transaction associated with the connection.
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Begins a transaction in the <see cref="Connection"/> source.
        /// It's useful to garantee the consistency of all operations made within
        /// the transaction, enabling rolling back all scripts ran in case of error.
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
    }
}
