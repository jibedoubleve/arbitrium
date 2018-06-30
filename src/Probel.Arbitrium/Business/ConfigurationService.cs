using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Admin;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Business
{
    public class ConfigurationService : IConfigurationService
    {
        #region Fields

        private readonly PollContext _pollContext;

        #endregion Fields

        #region Constructors

        public ConfigurationService(PollContext pollContext)
        {
            _pollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> GetRegistrationStatusAsync()
        {
            var e = await (from s in _pollContext.Settings
                           where s.Key.ToLower() == ConfigKeys.RegistrationStatus
                           select s.Value).SingleOrDefaultAsync();

            return (e == null) ? true : (e.ToLower() == "enabled");
        }

        public async Task UpdateAsync(string key, string value)
        {
            var setting = await (from s in _pollContext.Settings
                                 where s.Key.ToLower() == ConfigKeys.RegistrationStatus
                                 select s).SingleOrDefaultAsync();
            if (setting != null)
            {
                setting.Value = value;
                _pollContext.Update(setting);
                await _pollContext.SaveChangesAsync();
            }
            else if (key.ToLower() == ConfigKeys.RegistrationStatus)
            {
                var s = new Setting
                {
                    Key = ConfigKeys.RegistrationStatus,
                    Value = value,
                };
                _pollContext.Settings.Add(s);
                await _pollContext.SaveChangesAsync();
            }
        }

        public async Task<ConfigurationViewModel> GetFullConfiguration()
        {
            var r = new ConfigurationViewModel { IsRegistrationEnabled = true };

            var list = await _pollContext.Settings.ToListAsync();

            var k = (from kv in list
                     where kv.Key == ConfigKeys.RegistrationStatus
                     select kv.Value.ToLower()).SingleOrDefault();

            if (k == null)
            {
                _pollContext.Settings.Add(new Setting { Key = ConfigKeys.RegistrationStatus, Value = ConfigValues.Enabled });
                await _pollContext.SaveChangesAsync();
            }
            else { r.IsRegistrationEnabled = (k == ConfigValues.Enabled); }

            return r;
        }

        #endregion Methods
    }
}