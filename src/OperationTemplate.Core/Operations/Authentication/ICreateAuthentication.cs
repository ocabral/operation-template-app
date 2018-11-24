using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <summary>
    /// Operation responsible for create a new client application authentication.
    /// </summary>
    public interface ICreateAuthentication : IOperation<CreateAuthenticationRequest, CreateAuthenticationResponse>
    {
    }
}
