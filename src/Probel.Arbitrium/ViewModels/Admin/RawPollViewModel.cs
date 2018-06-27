using Probel.Arbitrium.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Probel.Arbitrium.ViewModels.Admin
{
    public class RawPollViewModel
    {
        #region Properties

        [Display(Name = "Choices")]
        public string Choices { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.DateTime)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(1);

        public long PollId { get; set; }

        [Display(Name = "Question")]
        public string Question { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.DateTime)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        public long UserId { get; set; }

        #endregion Properties
    }
}