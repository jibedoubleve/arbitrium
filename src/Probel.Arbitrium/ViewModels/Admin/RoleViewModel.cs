using System.Diagnostics;

namespace Probel.Arbitrium.ViewModels.Admin
{
    [DebuggerDisplay("{Name} - Selected: {IsSelected}")]
    public class RoleViewModel
    {
        #region Properties

        public bool IsSelected { get; set; }
        public string Name { get; set; }

        #endregion Properties
    }
}