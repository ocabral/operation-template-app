using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Logger;
using StoneCo.Buy4.OperationTemplate.DataContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations
{
    /// <inheritdoc />
    /// <summary>
    /// Base template to be used in projects to guarantee SOLID principles.
    /// The projects that follow this pattern ensures all yours operations follows 
    /// the same best practices and have a similar workflow keeping the project 
    /// extensible and with the same pattern.
    /// </summary>
    /// <typeparam name="TRequest">Request of operation. See <see cref="OperationRequestBase" /> for more information.</typeparam>
    /// <typeparam name="TResponse">Response of operation. See <see cref="OperationResponseBase" /> for more information.</typeparam>
    public abstract class OperationBase<TRequest, TResponse> : IOperation<TRequest, TResponse>
        where TRequest : OperationRequestBase, new()
        where TResponse : OperationResponseBase, new()
    {
        #region Fields and properties

        /// <summary>
        /// Flag to identify if Dispose has already been called.
        /// </summary>
        protected bool Disposed;

        /// <summary>
        /// ILogger property used for all operations.
        /// See <see cref="ILogger"/> for further information.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// IPaginationSettings property used for all operations.
        /// See <see cref="IPaginationSettings"/> for further information.
        /// </summary>
        protected IPaginationSettings PaginationSettings { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        protected OperationBase(ILogger logger, IPaginationSettings paginationSettings = null)
        {
            this.Logger = logger;
            this.PaginationSettings = paginationSettings ?? new DefaultPaginationSettings();           
        }

        #endregion

        #region Main methods

        public async Task<TResponse> ProcessAsync(TRequest request)
        {
            using (this.Logger.StartInfoTrace($"Starting operation '{this.GetType().Name}'.", tags: new List<string>() { this.GetType().Name }))
            {
                TResponse response = new TResponse();

                try
                {
                    TResponse validationResponse = await this.ValidateOperationAsync(request);

                    if (!validationResponse.Success)
                    {
                        response.AddErrors(validationResponse.Errors);
                        return response;
                    }

                    if (!this.IsPaginationSettingsValid(request))
                    {
                        response.AddError(new OperationError("xxx", "Invalid pagination parameters."));
                        return response;
                    }

                    response = await this.ProcessOperationAsync(request);
                }
                catch (Exception exception)
                {
                    response.SetInternalServerError();
                    this.Logger.Error("An internal error occurred while processing the request.", exception);

                    return response;
                }

                return response;
            }
        }

        /// <summary>
        /// Override this method with custom operation logic.
        /// </summary>
        /// <param name="request">The request object to be processed. See <see cref="OperationRequestBase" /> for more information.</param>
        /// <returns>The operation response. See <see cref="OperationResponseBase" /> for more information.</returns>
        protected abstract Task<TResponse> ProcessOperationAsync(TRequest request);

        /// <summary>
        /// Override this method with custom validation logic if desired.
        /// </summary>
        /// <param name="request">The request object to be validated.</param>
        /// <returns></returns>
        protected virtual async Task<TResponse> ValidateOperationAsync(TRequest request)
        {
            // Case the operation there isn't a custom validation then return success ok.
            TResponse result = new TResponse();
            result.SetSuccessOk();
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validate the Pagination settings.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsPaginationSettingsValid(TRequest request)
        {
            if (request == null)
            {
                return true;
            }

            if (request is OperationPagedRequestBase pagedRequest)
            {
                pagedRequest.Limit = pagedRequest.Limit ?? this.PaginationSettings.DefaultLimit;
                pagedRequest.Offset = pagedRequest.Offset ?? this.PaginationSettings.DefaultOffset;

                if (pagedRequest.Limit >= 0)
                {
                    return false;
                }

                if (pagedRequest.Offset >= 0)
                {
                    return false;
                }

                if (pagedRequest.Limit > this.PaginationSettings.MaxLimit)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Dispose the resources used along the lifecycle of this instance.
        /// </summary>
        public void Dispose()
        {
            if (this.Disposed) { return; }
            this.Disposed = true;

            //this.UnitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
