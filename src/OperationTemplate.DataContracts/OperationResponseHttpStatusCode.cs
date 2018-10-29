namespace StoneCo.Buy4.OperationTemplate.DataContracts
{
    /// <summary>
    /// For further information of Http status code below see https://www.restapitutorial.com/httpstatuscodes.html.
    /// </summary>
    public enum OperationResponseHttpStatusCode
    {
        // Success (2xx).
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,

        // Client error (4xx).
        BadRequest = 400,
        Forbidden = 403,
        NotAcceptable = 406,
        PreConditionFailed = 412,

        // Server error (5xx).
        InternalServerError = 500,
        ServiceUnavailable = 503,
    }
}
