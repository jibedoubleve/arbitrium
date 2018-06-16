using System.Collections.Generic;

namespace Probel.Arbitrium.Model
{
    public class User : Model
    {
        #region Properties

        public IEnumerable<Decision> Decisions { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        #endregion Properties
    }
}