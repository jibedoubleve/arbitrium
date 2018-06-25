using Probel.Arbitrium.Models;

namespace Probel.Arbitrium.ViewModels.Polls
{
    public class VoteViewModel
    {
        #region Properties

        public long ChoiceId { get; set; }
        public Poll Poll { get; set; }

        #endregion Properties
    }
}