using StoneCo.Buy4.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger
{
    /// <inheritdoc />
    /// <summary>
    /// Stone Log base class.
    /// </summary>
    public class StoneLogger : ILogger
    {
        /// <inheritdoc />
        /// <summary>
        /// Event triggered before Buy4 Log write a batch of log entries.
        /// </summary>
        public event Action<IList<LogEntry>> Logging;

        /// <inheritdoc />
        /// <summary>
        /// Event triggered when there is an error while Buy4 Log write a batch of log entries.
        /// </summary>
        public event Action<IList<LogEntry>, Exception> ErrorLogging;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoneLogger()
        {
            StoneCo.Buy4.Infrastructure.Logging.Logger.Logging += this.StoneLoggerLoggingListener;
            StoneCo.Buy4.Infrastructure.Logging.Logger.ErrorLogging += this.StoneLoggerErrorLoggingListener;
        }

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        /// Write information message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        public void Info(string message, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            this.Write(LogSeverity.Info, message, null, additionalData, tags);
        }

        /// <inheritdoc />
        /// <summary>
        /// Write debug message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        public void Debug(string message, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            this.Write(LogSeverity.Debug, message, null, additionalData, tags);
        }

        /// <inheritdoc />
        /// <summary>
        /// Write warning message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        public void Warn(string message, Exception exception = null, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            this.Write(LogSeverity.Warning, message, exception, additionalData, tags);
        }

        /// <inheritdoc />
        /// <summary>
        /// Write error message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="additionalData">Additional data to be logged.</param>
        /// <param name="tags">Tags to be logged.</param>
        public void Error(string message, Exception exception = null, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            this.Write(LogSeverity.Error, message, exception, additionalData, tags);
        }

        /// <inheritdoc />
        /// <summary>
        /// Create and start a performance trace, stopped when disposed.
        /// </summary>
        /// <param name="message">Message to be logged in the trace.</param>
        /// <param name="target"></param>
        /// <param name="additionalData">Additional data to be logged in the trace.</param>
        /// <param name="tags">Tags to be logged in the trace.</param>
        /// <returns>Running trace.</returns>
        public ILogTrace StartPerformanceTrace(string message, string target, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            return this.StartTrace(LogTraceType.PerformanceTrace, message, LogSeverity.Info, target, additionalData, tags);
        }

        /// <inheritdoc />
        /// <summary>
        /// Create and start an activity trace, stopped when disposed.
        /// </summary>
        /// <param name="message">Message to be logged in the trace.</param>
        /// <param name="target"></param>
        /// <param name="additionalData">Additional data to be logged in the trace.</param>
        /// <param name="tags">Tags to be logged in the trace.</param>
        /// <returns>Running trace.</returns>
        public ILogTrace StartActivityTrace(string message, string target, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            return this.StartTrace(LogTraceType.ActivityTrace, message, LogSeverity.Info, target, additionalData, tags);
        }

        /// <summary>
        /// Logs database exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <remarks>SQL Server error codes: "https://docs.microsoft.com/en-us/azure/sql-database/sql-database-develop-error-messages"</remarks>
        public void DatabaseError(Exception exception)
        {
            if (exception is SqlException sqlException)
            {
                int sqlErrorNumber = sqlException.Number;
                this.Error("A database error has occurred while processing the request.", exception: exception, additionalData: new Dictionary<string, object> { { "DatabaseErrorType", sqlErrorNumber } }, tags: new List<string> { "DatabaseError" });
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create and start a trace, stopped when disposed.
        /// </summary>
        /// <param name="logSeverity">Log severity.</param>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Log exception.</param>
        /// <param name="additionalData">Additional data to be logged in the trace.</param>
        /// <param name="tags">Tags to be logged in the trace.</param>
        private void Write(LogSeverity logSeverity, string message, Exception exception, IDictionary<string, object> additionalData, IEnumerable<string> tags)
        {
            LogEntry logEntry = exception != null ? ExceptionLogEntry.New(exception) : LogEntry.New();

            logEntry = logEntry.SetMessage(message);
            logEntry = logEntry.SetSeverity(logSeverity);

            if (additionalData != null)
            {
                logEntry = logEntry.SetAdditionalData(additionalData);
            }

            if (tags != null)
            {
                logEntry = logEntry.AddTags(tags);
            }

            StoneCo.Buy4.Infrastructure.Logging.Logger.Log(logEntry);
        }

        /// <summary>
        /// Create and start a trace, stopped when disposed.
        /// </summary>
        /// <param name="traceType"></param>
        /// <param name="message">Message to be logged in the trace.</param>
        /// <param name="logSeverity">Log severity.</param>
        /// <param name="target"></param>
        /// <param name="additionalData">Additional data to be logged in the trace.</param>
        /// <param name="tags">Tags to be logged in the trace.</param>
        /// <param name="skipBegin">If true, trace will log only the end (PerformanceTrace); if false, trace will log start and end (ContextTrace)</param>
        /// <returns>Running trace.</returns>
        private ILogTrace StartTrace(LogTraceType traceType, string message, LogSeverity logSeverity, string target, IDictionary<string, object> additionalData = null, IEnumerable<string> tags = null)
        {
            LogTrace logTrace = null;

            switch (traceType)
            {
                case LogTraceType.ActivityTrace:
                    logTrace = ActivityTrace.New(message).SetSeverity(logSeverity);
                    break;
                case LogTraceType.ContextTrace:
                    logTrace = ContextTrace.New(message).SetSeverity(logSeverity);
                    break;
                case LogTraceType.PerformanceTrace:
                    logTrace = PerformanceTrace.New(message).SetSeverity(logSeverity);
                    break;
                default:
                    logTrace = ContextTrace.New(message).SetSeverity(logSeverity);
                    break;
            }

            if (string.IsNullOrWhiteSpace(target) == false)
            {
                logTrace.SetTarget(target);
            }

            if (additionalData != null)
            {
                logTrace.SetAdditionalData(additionalData);
            }

            if (tags != null)
            {
                logTrace = logTrace.AddTags(tags);
            }

            return new StoneLogTrace(logTrace, logTrace.Start());
        }

        private void StoneLoggerLoggingListener(IList<LogEntry> logEntries)
        {
            Logging?.Invoke(logEntries);
        }

        private void StoneLoggerErrorLoggingListener(IList<LogEntry> logEntries, Exception exception)
        {
            ErrorLogging?.Invoke(logEntries, exception);
        }

        #endregion
    }
}
