namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication
{
    /// <summary>
    /// Request to create an Authentication.
    /// </summary>
    public class CreateAuthenticationRequest : OperationRequestBase
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
