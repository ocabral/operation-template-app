namespace StoneCo.Buy4.OperationTemplate.Core.Configurations
{
    public class DefaultPaginationSettings : IPaginationSettings
    {
        /// <summary>
        /// Default itens per page.
        /// </summary>
        public int DefaultLimit => 25;

        /// <summary>
        /// Default offset size for paging.
        /// </summary>
        public long DefaultOffset => 25;

        /// <summary>
        /// Maximum itens per page.
        /// </summary>
        public int MaxLimit => 1000;
    }
}
