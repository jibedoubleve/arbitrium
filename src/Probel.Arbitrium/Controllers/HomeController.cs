using Microsoft.AspNetCore.Mvc;
using Probel.Arbitrium.Model;
using Probel.Arbitrium.Services;

namespace Probel.Arbitrium.Controllers
{
    public class HomeController : Controller
    {
        private ILogService Log = new LogService();
        #region Methods

        public string Error()
        {
            return $"An error occured!";
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            Log.Debug($"'{login}' tries to connect with password '{password}'");

            return RedirectToAction("Polls");
        }

        [HttpGet]
        public IActionResult Polls()
        {
            var polls = new PollService().GetPolls();
            return View(polls);
        }

        #endregion Methods
    }
}