using System.ComponentModel.DataAnnotations;

namespace Probel.Arbitrium.ViewModels.Login
{
    public class NewLoginViewModel
    {
        #region Properties
        public string UserName { get; set; }

        [EmailAddress]
        public string Login { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        #endregion Properties
    }
}