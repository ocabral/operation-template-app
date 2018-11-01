namespace StoneCo.Buy4.OperationTemplate.Core.Configurations
{
    public class DefaultDatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; }

        public int QueryTimeoutInSeconds { get; private set; }

        public DefaultDatabaseSettings(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.QueryTimeoutInSeconds = 30;
        }

        public DefaultDatabaseSettings(string connectionString, int queryTimeoutInSeconds)
        {
            this.ConnectionString = connectionString;
            this.QueryTimeoutInSeconds = queryTimeoutInSeconds;
        }
    }
}
