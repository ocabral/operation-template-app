using StoneCo.Buy4.OperationTemplate.Core.Models.Authentication;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Infrastructure.DatabaseProvider.Repositories
{
    /// <summary>
    /// Repository to handle all authentication operations on database.
    /// </summary>
    public interface IAuthenticationRepository
    {
        /// <summary>
        /// Get Authentication by filter.
        /// </summary>
        /// <param name="request">Request filter.</param>
        /// <returns></returns>
        Task<IList<AuthenticationModel>> GetByFilter(GetAuthenticationsRequest request);

        /// <summary>
        /// Add a new authenticatio into database.
        /// </summary>
        /// <param name="authentication"></param>
        /// <returns></returns>
        Task<AuthenticationModel> Insert(AuthenticationModel authentication);

        /// <summary>
        /// Updates an authentication to active or inactive.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="activate"></param>
        /// <returns>Number of affected rows.</returns>
        Task<int> UpdateActivation(string applicationKey, bool activate);
    }
}
