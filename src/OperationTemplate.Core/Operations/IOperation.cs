using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using System;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations
{
    /// <summary>
    /// Base template to be used in projects to guarantee SOLID principles.
    /// The projects that follow this pattern ensures all yours operations follows 
    /// the same best practices and have a similar workflow keeping the project 
    /// extensible and with the same pattern.
    /// </summary>
    /// <typeparam name="TRequest">Request of operation. See <see cref="OperationRequestBase" /> for more information.</typeparam>
    /// <typeparam name="TResponse">Response of operation. See <see cref="OperationResponseBase" /> for more information.</typeparam>
    public interface IOperation<TRequest, TResponse> : IDisposable
        where TRequest : OperationRequestBase, new()
        where TResponse : OperationResponseBase, new()
    {
        /// <summary>
        /// Process the operation. The implementation must apply the processing logic.
        /// </summary>
        /// <param name="request">The request object to be processed. See <see cref="OperationRequestBase" /> for more information.</param>
        /// <returns>The operation response. See <see cref="OperationResponseBase" /> for more information.</returns>
        Task<TResponse> ProcessAsync(TRequest request);
    }

    /// <summary>
    /// Base template to be used in projects to guarantee SOLID principles.
    /// The projects that follow this pattern ensures all yours operations follows 
    /// the same best practices and have a similar workflow keeping the project 
    /// extensible and with the same pattern.
    /// </summary>
    /// <typeparam name="TRequest">Request of operation. See <see cref="OperationRequestBase" /> for more information.</typeparam>
    /// <typeparam name="TResponse">Response of operation. See <see cref="OperationResponseBase" /> for more information.</typeparam>
    /// <typeparam name="TValidation">Validation to validate the request of operation. See <see cref="ValidationBase{TRequest, TResponse}"/> for more information.</typeparam>
    public interface IOperation<TRequest, TResponse, TValidation> : IDisposable
        where TRequest : OperationRequestBase, new()
        where TResponse : OperationResponseBase, new()
        where TValidation : IValidation<TRequest, TResponse>, new()
    {
        /// <summary>
        /// Process the operation. The implementation must apply the processing logic.
        /// </summary>
        /// <param name="request">The request object to be processed. See <see cref="OperationRequestBase" /> for more information.</param>
        /// <returns>The operation response. See <see cref="OperationResponseBase" /> for more information.</returns>
        Task<TResponse> ProcessAsync(TRequest request);
    }
}
