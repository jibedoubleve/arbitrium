using System.Collections.Generic;

namespace Probel.Arbitrium.Model
{
    public class PollService
    {
        #region Methods

        public IEnumerable<Poll> GetPolls()
        {
            using (var db = new PollContext())
            {
                return db.Polls;
            }
        }

        #endregion Methods
    }
}