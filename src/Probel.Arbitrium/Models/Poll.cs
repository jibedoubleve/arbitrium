using Probel.Arbitrium.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Probel.Arbitrium.Models
{
    public class Poll : Model
    {
        #region Properties

        public IEnumerable<Choice> Choices { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.DateTime)]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public DateTime EndDateLocal => EndDate.ToLocalTime();

        public string Question { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.DateTime)]
        public DateTime StartDate { get; set; }

        [NotMapped]
        public DateTime StartDateLocal => StartDate.ToLocalTime();

        [NotMapped]
        public bool IsOpen => (DateTime.Now.ToUniversalTime() <= EndDate);

        [NotMapped]
        public bool IsStarted => (DateTime.Now.ToUniversalTime() > StartDate);

        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.TimeSpan)]
        public TimeSpan RemainingTimeBeforeEnd => StartDate - DateTime.Now.ToUniversalTime();

        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.TimeSpan)]
        public TimeSpan RemainingTimeBeforeStart => EndDate - DateTime.Now.ToUniversalTime();

        #endregion Properties
    }
}