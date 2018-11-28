using FluentAssertions;
using Moq;
using Nerdle.AutoConfig;
using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoneCo.Buy4.OperationTemplate.UnitTest.Core.Infrastructure.DatabaseProvider
{
    public class SqlServerDbConnectorTest
    {
        [Fact]
        public void SqlServerDbConnector_ShouldCreateSqlConnection_WhenConstructorIsCalled()
        {
            // Act
            SqlServerDbConnector sqlDbConnector = new SqlServerDbConnector("data source=localhost,31434;initial catalog=DatabaseName;persist security info=False;User Id=AppUser; Password=AppPassword");

            // Assert
            sqlDbConnector.Connection.Should().NotBeNull();
        }

        [Fact]
        public void SqlServerDbConnector_ShouldNotOpenSqlConnection_WhenConstructorIsCalled()
        {
            // Act
            SqlServerDbConnector sqlDbConnector = new SqlServerDbConnector("data source=localhost,31434;initial catalog=DatabaseName;persist security info=False;User Id=AppUser; Password=AppPassword");

            // Assert
            sqlDbConnector.Connection.State.Should().BeEquivalentTo(ConnectionState.Closed);
        }

        [Fact]
        public void BeginTransaction_ShouldCreateTransaction_WhenBeginTransactionIsCalled()
        {
            // Arrange
            Mock<IDbConnection> mockedConnection = new Mock<IDbConnection>();

            Mock<SqlServerDbConnector> sqlServerDbConnector = new Mock<SqlServerDbConnector>("data source=localhost,31434;initial catalog=DatabaseName;persist security info=False;User Id=AppUser; Password=AppPassword");
            sqlServerDbConnector.Setup(x => x.Connection).Returns(mockedConnection.Object);

            // Act
            sqlServerDbConnector.Object.BeginTransaction();

            // Assert
            mockedConnection.Verify(x => x.BeginTransaction(), Times.Once());
        }

        [Fact]
        public void BeginTransaction_ShouldCommitTransaction_WhenCommitTransactionIsCalled()
        {
            // Arrange
            Mock<IDbConnection> mockedConnection = new Mock<IDbConnection>();
            mockedConnection.SetupGet(x => x.State).Returns(ConnectionState.Open);

            Mock<IDbTransaction> mockedTransaction = new Mock<IDbTransaction>();
            mockedTransaction.SetupGet(x => x.Connection).Returns(mockedConnection.Object);

            Mock<SqlServerDbConnector> sqlServerDbConnector = new Mock<SqlServerDbConnector>("data source=localhost,31434;initial catalog=DatabaseName;persist security info=False;User Id=AppUser; Password=AppPassword");
            sqlServerDbConnector.Setup(x => x.Connection).Returns(mockedConnection.Object);

            // Act
            sqlServerDbConnector.Object.BeginTransaction();
            sqlServerDbConnector.Setup(x => x.Transaction).Returns(mockedTransaction.Object);

            sqlServerDbConnector.Object.CommitTransaction();

            // Assert
            mockedTransaction.Verify(x => x.Commit(), Times.Once());
        }

        [Fact]
        public void BeginTransaction_ShouldRollbackTransaction_WhenRollbackTransactionIsCalled()
        {
            // Arrange
            Mock<IDbConnection> mockedConnection = new Mock<IDbConnection>();
            mockedConnection.SetupGet(x => x.State).Returns(ConnectionState.Open);

            Mock<IDbTransaction> mockedTransaction = new Mock<IDbTransaction>();
            mockedTransaction.SetupGet(x => x.Connection).Returns(mockedConnection.Object);

            Mock<SqlServerDbConnector> sqlServerDbConnector = new Mock<SqlServerDbConnector>("data source=localhost,31434;initial catalog=DatabaseName;persist security info=False;User Id=AppUser; Password=AppPassword");
            sqlServerDbConnector.Setup(x => x.Connection).Returns(mockedConnection.Object);

            // Act
            sqlServerDbConnector.Object.BeginTransaction();
            sqlServerDbConnector.Setup(x => x.Transaction).Returns(mockedTransaction.Object);

            sqlServerDbConnector.Object.RollbackTransaction();

            // Assert
            mockedTransaction.Verify(x => x.Rollback(), Times.Once());
        }

        [Fact]
        public void Dispose_ShouldCloseAllDatabaseConnectionsAndDisposeTransaction_WhenIsCalled()
        {
            // Arrange
            Mock<IDbConnection> mockedConnection = new Mock<IDbConnection>();
            mockedConnection.SetupGet(x => x.State).Returns(() => ConnectionState.Open);

            Mock<SqlServerDbConnector> sqlServerDbConnector = new Mock<SqlServerDbConnector>("data source=localhost,31434;initial catalog=DatabaseName;persist security info=False;User Id=AppUser; Password=AppPassword");
            sqlServerDbConnector.Setup(x => x.Connection).Returns(mockedConnection.Object);
            sqlServerDbConnector.Setup(x => x.Transaction.Connection).Returns(mockedConnection.Object);

            // Act
            sqlServerDbConnector.Object.Dispose();

            // Assert
            mockedConnection.Verify(x => x.Close(), Times.Exactly(2));
        }
    }
}
