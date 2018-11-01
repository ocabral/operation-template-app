using System;
using System.Collections.Generic;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger
{
    /// <inheritdoc />
    public interface ILogTrace : IDisposable
    {
        /// <summary>
        /// Set additional data to be logged in a trace.
        /// </summary>
        /// <param name="additionalData">Additional data to be logged in a trace</param>
        void SetAdditionalData(IDictionary<string, object> additionalData);

        /// <summary>
        /// Adds additional data to be logged in a trace.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="value">Value</param>
        void AddAdditionalData(string name, object value);
    }
}
