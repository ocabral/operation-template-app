using StoneCo.Buy4.OperationTemplate.Core.Configurations;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider;
using StoneCo.Buy4.OperationTemplate.Core.Infrastructure.Logger;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
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
        /// Flag to identify if dispose has already been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// ILogger property used for all operations.
        /// See <see cref="ILogger"/> for more information.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// IPaginationSettings property used for all operations.
        /// See <see cref="IPaginationSettings"/> for more information.
        /// </summary>
        protected IPaginationSettings PaginationSettings { get; }

        /// <summary>
        /// Unit of Work responsible for handle the repositories.
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        protected OperationBase(ILogger logger, IPaginationSettings paginationSettings = null, IUnitOfWork unitOfWork = null)
        {
            this.Logger = logger;
            this.PaginationSettings = paginationSettings ?? new DefaultPaginationSettings();
            this.UnitOfWork = unitOfWork;
        }

        #endregion

        #region Main methods

        public async Task<TResponse> ProcessAsync(TRequest request)
        {
            try
            {
                using (this.Logger?.StartInfoTrace($"Starting operation '{this.GetType().Name}'.", tags: new List<string>() { this.GetType().Name }))
                {
                    TResponse response = new TResponse();

                    TResponse validationResponse = await this.ValidateOperationAsync(request);

                    if (validationResponse.Success == false)
                    {
                        response.AddErrors(validationResponse.Errors);
                        return response;
                    }

                    if (this.IsPaginationSettingsValid(request) == false)
                    {
                        response.AddError(new OperationError("xxx", "Invalid pagination parameters."));
                        return response;
                    }

                    response = await this.ProcessOperationAsync(request);

                    return response;
                }
            }
            catch (Exception exception)
            {
                TResponse errorResponse = new TResponse();
                errorResponse.SetInternalServerError();

                this.Logger?.Error("An internal error occurred while processing the request.", exception, tags: new List<string>() { this.GetType().Name });

                return errorResponse;
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
        /// <param name="request">Request of operation. See <see cref="OperationRequestBase" /> for more information.</param>
        /// <returns>Response of validation. See <see cref="OperationResponseBase" /> for more information.</returns>
        protected virtual async Task<TResponse> ValidateOperationAsync(TRequest request)
        {
            // Case the operation doesn't have a custom validation then return success = true.
            TResponse result = new TResponse();
            result.SetSuccessOk();

            return await Task.FromResult(result);

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

                if (pagedRequest.Limit < 0)
                {
                    return false;
                }

                if (pagedRequest.Offset < 0)
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
            if (this.disposed) { return; }

            this.disposed = true;
            this.UnitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    /// Base template to be used in projects to guarantee SOLID principles.
    /// The projects that follow this pattern ensures all yours operations follows 
    /// the same best practices and have a similar workflow keeping the project 
    /// extensible and with the same pattern.
    /// </summary>
    /// <typeparam name="TRequest">Request of operation. See <see cref="OperationRequestBase" /> for more information.</typeparam>
    /// <typeparam name="TResponse">Response of operation. See <see cref="OperationResponseBase" /> for more information.</typeparam>
    /// <typeparam name="TValidation">Validation to validate the request of operation. See <see cref="ValidationBase{TRequest, TResponse}"/> for more information.</typeparam>
    public abstract class OperationBase<TRequest, TResponse, TValidation> : OperationBase<TRequest, TResponse>
        where TRequest : OperationRequestBase, new()
        where TResponse : OperationResponseBase, new()
        where TValidation : IValidation<TRequest, TResponse>, new()
    {
        /// <inheritdoc />
        protected OperationBase(ILogger logger, IPaginationSettings paginationSettings = null)
            : base(logger, paginationSettings)
        {
        }

        /// <inheritdoc />
        protected sealed override async Task<TResponse> ValidateOperationAsync(TRequest request) => await (new TValidation()).ValidateOperationAsync(request);
    }
}
