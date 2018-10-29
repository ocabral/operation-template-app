using System;
using System.Collections.Generic;

namespace StoneCo.Buy4.OperationTemplate.Core.Logger
{
    public interface ILogger
    {
        /// <summary>
        /// Write information message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        void Info(string message, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null);

        /// <summary>
        /// Write debug message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        void Debug(string message, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null);

        /// <summary>
        /// Write warning message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        void Warn(string message, Exception exception = null, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null);

        /// <summary>
        /// Write error message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        void Error(string message, Exception exception = null, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null);

        /// <summary>
        /// Create and start a trace, stopped when disposed.
        /// </summary>
        /// <param name="message">Message to be logged in the trace.</param>
        /// <param name="additionalData">Additional data to be logged in the trace.</param>
        /// <param name="skipBegin">If true, trace will log only the end (PerformanceTrace); if false, trace will log start and end (ContextTrace)</param>
        /// <param name="tags">Tags to be logged.</param>
        /// <returns>Running trace.</returns>
        ILogTrace StartInfoTrace(string message, IDictionary<string, object> additionalData = null, bool skipBegin = false, IEnumerable<string> tags = null);

        /// <summary>
        /// Create and start a debug trace, stopped when disposed.
        /// </summary>
        /// <param name="message">Message to be logged in the trace.</param>
        /// <param name="additionalData">Additional data to be logged in the trace.</param>
        /// <param name="skipBegin">If true, trace will log only the end (PerformanceTrace); if false, trace will log start and end (ContextTrace)</param>
        /// <param name="tags">Tags to be logged.</param>
        /// <returns>Running trace.</returns>
        ILogTrace StartDebugTrace(string message, IDictionary<string, object> additionalData = null, bool skipBegin = false, IEnumerable<string> tags = null);
    }
}
