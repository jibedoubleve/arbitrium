using Probel.Arbitrium.Models;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class VoteViewModel
    {
        #region Properties

        public long UserId { get; set; }
        public Poll Poll { get; set; }

        #endregion Properties
    }
}