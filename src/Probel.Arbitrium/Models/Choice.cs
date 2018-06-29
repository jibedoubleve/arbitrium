using System.Collections.Generic;

namespace Probel.Arbitrium.Models
{
    public class Choice : Model
    {
        #region Properties

        public Poll Poll { get; set; }

        public string Text { get; set; }

        public IEnumerable<Decision> Decisions { get; set; }

        #endregion Properties
    }
}