using System;

namespace Probel.Arbitrium.Model
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