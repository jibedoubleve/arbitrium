using System.Diagnostics;

namespace Probel.Arbitrium.Services
{
    public class LogService : ILogService
    {
        #region Methods

        public void Debug(string msg) => Trace.WriteLine(msg);

        #endregion Methods
    }
}