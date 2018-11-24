namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication
{
    public class ValidateAuthenticationResponse : OperationResponseBase
    {
        public bool IsValid { get; set; }

        public string ApplicationName { get; set; }
    }
}
