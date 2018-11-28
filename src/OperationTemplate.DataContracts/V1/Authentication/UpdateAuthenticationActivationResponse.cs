namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication
{
    public class UpdateAuthenticationActivationResponse : OperationResponseBase
    {
        /// <summary>
        /// Application key.
        /// All applications must inform the application key to authenticate.
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// Indicate if the authentication is active / enable.
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
