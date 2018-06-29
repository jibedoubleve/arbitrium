using Probel.Arbitrium.Business;
using System.Collections.Generic;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class UserRoleViewModel
    {
        #region Fields

        public IEnumerable<string> Roles;

        #endregion Fields

        #region Constructors

        public UserRoleViewModel()
        {
            Roles = new List<string>
            {
                RoleList.Admin,
                RoleList.SuperUser,
            };
            UserRoles = new List<RoleViewModel>();
        }

        #endregion Constructors

        #region Properties

        public IList<RoleViewModel> UserRoles { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }

        #endregion Properties
    }
}