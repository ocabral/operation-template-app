namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1
{
    /// <summary>
    /// Class that can be used to hold results from a method execution, like statuses, validation errors and related stuff.
    /// </summary>
    public class OperationError
    {
        /// <summary>
        /// Class' constructor.
        /// </summary>
        /// <param name="code">Operation error code.</param>
        /// <param name="message">Operation error message.</param>
        public OperationError(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// Operation error output message.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Field source.
        /// </summary>
        public string Code { get; protected set; }
    }
}

