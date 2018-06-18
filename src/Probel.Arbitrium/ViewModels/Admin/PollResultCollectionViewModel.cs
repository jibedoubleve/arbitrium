using System.Collections.Generic;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class PollResultCollectionViewModel
    {
        #region Properties

        public IEnumerable<PollResultViewModel> PollResults { get; set; }
        public long UserId { get; set; }

        #endregion Properties
    }
}