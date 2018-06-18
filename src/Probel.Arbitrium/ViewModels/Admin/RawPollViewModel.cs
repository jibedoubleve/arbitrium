using System.ComponentModel.DataAnnotations;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class RawPollViewModel
    {
        #region Properties

        [Display(Name = "Choices")]
        public string Choices { get; set; }

        [Display(Name = "Question")]
        public string Question { get; set; }

        public long UserId { get; set; }

        public long PollId { get; set; }
        #endregion Properties
    }
}