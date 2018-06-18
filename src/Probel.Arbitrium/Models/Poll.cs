using System;
using System.Collections.Generic;

namespace Probel.Arbitrium.Models
{
    public class Poll : Model
    {
        #region Properties

        public IEnumerable<Choice> Choices { get; set; }

        public string Question { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion Properties
    }
}