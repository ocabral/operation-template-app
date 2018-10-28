using StoneCo.Buy4.OperationTemplate.DataContracts;
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
    /// <typeparam name="TRequest">Request to the operation.</typeparam>
    /// <typeparam name="TResponse">Response from the operation.</typeparam>
    public interface IOperation<TRequest, TResponse> : IDisposable
        where TRequest : OperationRequestBase
        where TResponse : OperationResponseBase, new()
    {
        /// <summary>
        /// Process the operation. The implementation must apply the processing logic.
        /// </summary>
        /// <param name="request">The request object to be processed. <see cref="OperationRequestBase" /> for more information.</param>
        /// <returns>The operation response. See <see cref="OperationResponseBase" /> for more information.</returns>
        Task<TResponse> ProcessAsync(TRequest request);
    }
}
