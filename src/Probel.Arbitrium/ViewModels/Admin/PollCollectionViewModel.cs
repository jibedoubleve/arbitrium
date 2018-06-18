using Probel.Arbitrium.Models;
using System.Collections.Generic;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class PollCollectionViewModel
    {
        #region Properties

        public IEnumerable<Poll> NewPolls { get; set; }
        public IEnumerable<Poll> OldPolls { get; set; }
        public long UserId { get; set; }

        #endregion Properties
    }
}