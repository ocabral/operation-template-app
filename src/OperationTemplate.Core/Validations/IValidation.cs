using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using System;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Validations
{
    public interface IValidation<TRequest, TResponse> : IDisposable
        where TRequest : OperationRequestBase, new()
        where TResponse : OperationResponseBase, new()
    {
        /// <summary>
        /// Override this method with custom validation logic if desired.
        /// </summary>
        /// <param name="request">Request of operation. See <see cref="OperationRequestBase" /> for more information.</param>
        /// <returns>Response of validation. See <see cref="OperationResponseBase" /> for more information.</returns>
        Task<TResponse> ValidateOperationAsync(TRequest request);
    }
}
