using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <summary>
    /// Operation responsible for return a client application authentication.
    /// </summary>
    public interface IGetAuthentication : IOperation<GetAuthenticationsRequest, GetAuthenticationResponse>
    {
    }
}
