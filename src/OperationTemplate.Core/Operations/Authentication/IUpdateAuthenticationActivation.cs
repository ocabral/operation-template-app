using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <summary>
    /// Operation responsible for update the authentication to active or inactive.
    /// </summary>
    public interface IUpdateAuthenticationActivation : IOperation<UpdateAuthenticationActivationRequest, UpdateAuthenticationActivationResponse>
    {
    }
}
