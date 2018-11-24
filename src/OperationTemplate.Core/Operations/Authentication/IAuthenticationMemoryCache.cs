using StoneCo.Buy4.OperationTemplate.Core.Commons;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <summary>
    /// IAuthenticatioMemoryCache interface.
    /// </summary>
    public interface IAuthenticatioMemoryCache : IMemoryCacheHelper<string, AuthenticationModel>
    {
    }
}
