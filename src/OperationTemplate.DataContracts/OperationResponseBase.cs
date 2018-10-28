using System.Collections.Generic;
using System.Linq;

namespace StoneCo.Buy4.OperationTemplate.DataContracts
{
    /// <summary>
    /// The base class to be used as response from a method execution.
    /// </summary>
    public class OperationResponseBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OperationResponseBase()
        {
            this.Success = false;
            this.Errors = null;
        }

        /// <summary>
        /// Default constructor.
        /// If 'errors' has one or more itens, the HttpStatusCode will be set to 400 (BadRequest) by default.
        /// </summary>
        public OperationResponseBase(bool sucess, List<OperationError> errors)
        {
            this.Success = sucess;
            this.Errors = errors;

            if(errors != null && errors.Count() > 0)
            {
                this.HttpStatusCode = OperationResponseHttpStatusCode.BadRequest;
            }
        }

        /// <summary>
        /// Indicates if operation completed without errors.
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// Detailed results of a method execution.
        /// </summary>
        public List<OperationError> Errors { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public OperationResponseHttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Add a error to response.
        /// The HttpStatusCode will be set to 400 (BadRequest) by default.
        /// </summary>
        /// <param name="operationError"><see cref="OperationError"/> object.</param>
        public void AddError(OperationError operationError)
        {
            (this.Errors ?? (this.Errors = new List<OperationError>())).Add(operationError);
            this.Success = false;
            this.HttpStatusCode = OperationResponseHttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Add an error list to response.
        /// The HttpStatusCode will be set to 400 (BadRequest) by default.
        /// </summary>
        /// <param name="operationInfos">List of <see cref="OperationError"/>.</param>
        public void AddErrors(List<OperationError> operationInfos)
        {
            (this.Errors ?? (this.Errors = new List<OperationError>())).AddRange(operationInfos);
            this.Success = false;
            this.HttpStatusCode = OperationResponseHttpStatusCode.BadRequest;
        }

        /// <summary>
        /// The HttpStatusCode will be set to 200 (Ok).
        /// </summary>
        public void SetSuccessOk()
        {
            this.Success = true;
            this.HttpStatusCode = OperationResponseHttpStatusCode.Ok;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents the return of an internal operation.
    /// </summary>
    /// <typeparam name="T">Type of data to be returned.</typeparam>
    public class OperationResponseBase<T> : OperationResponseBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OperationResponseBase() { }

        /// <inheritdoc />
        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="data">Response data object</param>
        public OperationResponseBase(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Response data object.
        /// </summary>
        public T Data { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents the return of an internal operation with pagination.
    /// </summary>
    /// <typeparam name="T">Type of data to be returned.</typeparam>
    public class OperationPagedResponseBase<T> : OperationResponseBase<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OperationPagedResponseBase() { }

        /// <inheritdoc />
        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="data">Response data object</param>
        /// <param name="limit">The limit of the response.</param>
        /// <param name="offset">The offset of the response</param>
        public OperationPagedResponseBase(T data, int limit, long offset)
        {
            this.Data = data;
            this.Limit = limit;
            this.Offset = offset;
        }

        /// <summary>
        /// Limit of the response.
        /// </summary>
        public virtual int? Limit { get; set; }

        /// <summary>
        /// Offset of the response.
        /// </summary>
        public virtual long? Offset { get; set; }
    }
}

