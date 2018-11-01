namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1
{
    /// <summary>
    /// Base class for request operations.
    /// </summary>
    public class OperationRequestBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OperationRequestBase() { }
    }

    /// <summary>
    /// Base class for request paged operations.
    /// </summary>
    public class OperationPagedRequestBase : OperationRequestBase
    {
        /// <summary>
        /// Request Resource limit in current page.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Requested Resource current page offset.
        /// </summary>
        public long? Offset { get; set; }
    }

    /// <summary>
    /// Base class for request operations.
    /// </summary>
    public class OperationRequestBase<T> : OperationRequestBase where T : class
    {
        /// <summary>
        /// Request Data.
        /// </summary>
        public T Data { get; set; }
    }
}
