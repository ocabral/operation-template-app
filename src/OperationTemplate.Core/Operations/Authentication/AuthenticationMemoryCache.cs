using StoneCo.Buy4.OperationTemplate.Core.Commons;
using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <inheritdoc />
    public class AuthenticationMemoryCache : MemoryCacheHelper<string, AuthenticationModel>, IAuthenticatioMemoryCache
    {
        /// <summary>
        /// AuthenticatioMemoryCache default constructor.
        /// </summary>
        public AuthenticationMemoryCache() : base()
        {
        }
    }
}
