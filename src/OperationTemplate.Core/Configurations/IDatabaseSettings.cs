namespace StoneCo.Buy4.OperationTemplate.Core.Configurations
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; }

        int QueryTimeoutInSeconds { get; }
    }
}
