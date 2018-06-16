using System.Collections.Generic;

namespace Probel.Arbitrium.Model
{
    public class Poll : Model
    {
        #region Properties

        public IEnumerable<Choice> Choices { get; set; }
        public string Question { get; set; }

        #endregion Properties
    }
}