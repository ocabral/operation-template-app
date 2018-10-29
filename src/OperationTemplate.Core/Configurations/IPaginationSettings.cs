namespace StoneCo.Buy4.OperationTemplate.Core.Configurations
{
    public interface IPaginationSettings
    {
        /// <summary>
        /// Default itens per page.
        /// </summary>
        int DefaultLimit { get; }

        /// <summary>
        /// Default offset size for paging.
        /// </summary>
        long DefaultOffset { get; }

        /// <summary>
        /// Maximum itens per page.
        /// </summary>
        int MaxLimit { get; }
    }
}
