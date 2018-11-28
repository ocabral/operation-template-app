using System;
using System.Data;
using System.Data.SqlClient;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider
{
    /// <summary>
    /// Responsible for handling a database connection, processing database transactions
    /// and ensuring the consistency of the transaction.
    /// </summary>
    public class SqlServerDbConnector : IDbConnector
    {
        private bool _disposed;

        /// <inheritdoc />
        public virtual IDbConnection Connection { get; }

        /// <inheritdoc />
        public virtual IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// SqlServerDbConnector public constructor.
        /// </summary>
        /// <param name="databaseConnectionString">Database connection string.</param>
        public SqlServerDbConnector(string databaseConnectionString)
        {
            this.Connection = new SqlConnection(databaseConnectionString);
        }

        /// <inheritdoc />
        public void BeginTransaction()
        {
            if (this.IsTransactionOpened())
            {
                throw new Exception("There is already an existing transaction for this connection.");
            }

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            this.Transaction = this.Connection.BeginTransaction();
        }

        /// <inheritdoc />
        public void CommitTransaction()
        {
            if (this.IsTransactionOpened())
            {
                this.Transaction.Commit();
            }
            else
            {
                throw new Exception("Transaction isn't opened. It must call BeginTransaction() before commit.");
            }
        }

        /// <inheritdoc />
        public void RollbackTransaction()
        {
            if (this.IsTransactionOpened())
            {
                this.Transaction.Rollback();
            }
            else
            {
                throw new Exception("Transaction isn't opened. It must call BeginTransaction() before rollback.");
            }
        }

        /// <summary>
        /// Dispose the resources used along the lifecycle of this instance.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;

            if (this.Connection != null && this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }

            if (this.Transaction != null)
            {
                if (this.Transaction.Connection != null && this.Transaction.Connection.State != ConnectionState.Closed)
                {
                    this.Transaction.Connection.Close();
                }

                this.Transaction = null;
            }

            GC.SuppressFinalize(this);
        }

        #region Private methods

        /// <summary>
        /// Determines whether [is transaction opened].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is transaction opened]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsTransactionOpened()
        {
            return this.Transaction?.Connection?.State == ConnectionState.Open;
        }

        #endregion
    }
}
