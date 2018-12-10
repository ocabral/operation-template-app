using StoneCo.Buy4.Infrastructure.Logging;
using System.Collections.Generic;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger
{
    /// <inheritdoc />
    /// <summary>
    /// Trace to be logged in Stone Log.
    /// </summary>
    public class StoneLogTrace : ILogTrace
    {
        private readonly LogTrace _logTrace;
        private readonly TraceLogger _traceLogger;

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="logTrace">Log information to write to the log entry</param>
        /// <param name="traceLogger">Base log trace class.</param>
        public StoneLogTrace(LogTrace logTrace, TraceLogger traceLogger)
        {
            this._logTrace = logTrace;
            this._traceLogger = traceLogger;
        }

        /// <inheritdoc />
        /// <summary>
        /// Set additional data to be logged in a trace.
        /// </summary>
        /// <param name="additionalData">Additional data to be logged in a trace</param>
        public void SetAdditionalData(IDictionary<string, object> additionalData)
        {
            this._logTrace.SetAdditionalData(additionalData);
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds additional data to be logged in a trace.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="value">Value</param>
        public void AddAdditionalData(string name, object value)
        {
            this._logTrace.AdditionalData.Add(new KeyValuePair<string, object>(name, value));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._traceLogger.Dispose();
        }
    }
}
