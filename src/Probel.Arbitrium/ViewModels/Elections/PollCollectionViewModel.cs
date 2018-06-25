using Probel.Arbitrium.Models;
using System.Collections.Generic;

namespace Probel.Arbitrium.ViewModels.Polls
{
    public class PollCollectionViewModel
    {
        #region Properties

        public IEnumerable<Poll> NewPolls { get; set; }
        public IEnumerable<Poll> OldPolls { get; set; }

        #endregion Properties
    }
}