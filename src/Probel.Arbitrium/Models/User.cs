using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Probel.Arbitrium.Models
{
    public class User : IdentityUser<long>
    {
        #region Properties
        public IEnumerable<Decision> Decisions { get; set; }

        #endregion Properties
    }
}