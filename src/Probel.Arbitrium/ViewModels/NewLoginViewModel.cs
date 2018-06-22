using System.ComponentModel.DataAnnotations;

namespace Probel.Arbitrium.ViewModels
{
    public class NewLoginViewModel
    {
        #region Properties

        [EmailAddress]
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

        #endregion Properties
    }
}