using Probel.Arbitrium.ViewModels.Admin;

namespace Probel.Arbitrium.ViewModels.Login
{
    public class LoginViewModel
    {
        #region Properties

        public string Login { get; set; }
        public string Password { get; set; }
        public ConfigurationViewModel Configuration { get; set; }
        #endregion Properties
    }
}