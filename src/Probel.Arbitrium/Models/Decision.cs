using System;

namespace Probel.Arbitrium.Models
{
    public class Decision : Model
    {
        #region Properties

        public Choice Choice { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }

        #endregion Properties
    }
}