using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <summary>
    /// Operation responsible for validate an authorization hash sent by client applications.
    /// </summary>
    public interface IValidateAuthentication : IOperation<ValidateAuthenticationRequest, ValidateAuthenticationResponse>
    {
    }
}
