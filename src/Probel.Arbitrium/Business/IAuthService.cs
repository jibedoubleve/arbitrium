using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Business
{
    public interface IAuthService
    {
        #region Methods

        Task<User> GetConnectedUserAsync();

        Task<long> GetConnectedUserIdAsync();

        Task<bool> HasConnectedUser();

        Task<IEnumerable<RoleViewModel>> GetRolesAsStringAsync(long userId);
        #endregion Methods
    }
}