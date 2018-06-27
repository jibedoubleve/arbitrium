using Probel.Arbitrium.Models;
using System.Collections.Generic;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class UserCollectionViewModel
    {
        #region Properties

        public IEnumerable<User> RegisteredUsers { get; set; }

        #endregion Properties
    }
}