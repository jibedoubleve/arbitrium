using Probel.Arbitrium.ViewModels.Admin;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Business
{
    public interface IConfigurationService
    {
        #region Methods

        Task<ConfigurationViewModel> GetFullConfiguration();

        Task<bool> GetRegistrationStatusAsync();

        Task UpdateAsync(string key, string value);

        #endregion Methods
    }

    public static class ConfigKeys
    {
        #region Fields

        public const string RegistrationStatus = "registration-status";

        #endregion Fields
    }

    public static class ConfigValues
    {
        #region Fields

        public static string Disabled = "disabled";
        public static string Enabled = "enabled";

        #endregion Fields
    }
}