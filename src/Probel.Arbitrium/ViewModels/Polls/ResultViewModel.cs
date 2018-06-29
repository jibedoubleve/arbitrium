using Probel.Arbitrium.Models;

namespace Probel.Arbitrium.ViewModels.Polls
{
    public class ResultViewModel
    {
        public Poll Poll { get; set; }
        public bool HasUserAnswer { get; set; }
    }
}