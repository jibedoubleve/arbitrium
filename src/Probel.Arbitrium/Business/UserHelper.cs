using System;

namespace Probel.Arbitrium.Business
{
    public class UserHelper
    {
        #region Methods

        public string RandomiseEmail()
        {
            var random = Guid.NewGuid().ToString();
            return $"{random}@gmail.com";
        }

        #endregion Methods
    }
}