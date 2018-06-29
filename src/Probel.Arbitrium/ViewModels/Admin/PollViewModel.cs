using Probel.Arbitrium.Models;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class PollViewModel
    {
        #region Properties

        public bool CanEdit { get; set; }
        public Poll Poll { get; set; }

        #endregion Properties
    }
}