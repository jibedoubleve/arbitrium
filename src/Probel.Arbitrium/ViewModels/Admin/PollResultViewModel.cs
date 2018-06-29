using System.Collections.Generic;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class PollResultViewModel
    {
        #region Properties
        public long UserId { get; set; }
        public IEnumerable<DecisionResultViewModel> Choices { get; set; }
        public string Question { get; set; }

        #endregion Properties
    }
}