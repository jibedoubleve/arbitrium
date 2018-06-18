using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Business;
using Probel.Arbitrium.Core.Exception;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.Services;
using Probel.Arbitrium.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    public class LoginController : Controller
    {
        #region Fields

        private readonly PollContext PollContext;

        private readonly ILogService Log = new LogService();

        #endregion Fields

        #region Constructors

        public LoginController(PollContext pollContext)
        {
            PollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        [HttpPost]
        public async Task<IActionResult> Account(string login)
        {
            var user = await (from p in PollContext.Users
                              where p.Login.ToLower() == login.ToLower()
                              select p).SingleOrDefaultAsync();


            var result = (user != null)
                ? RedirectToAction("Password", new PasswordViewModel { UserId = user.Id })
                : RedirectToAction("Create", new { login });
            return result;
        }

        [HttpGet]
        public IActionResult Account() => View();

        [HttpGet]
        public async Task<IActionResult> Password(long userId)
        {
            var user = await PollContext.Users.FindAsync(userId);
            return View(new PasswordViewModel() { UserId = userId });
        }

        [HttpPost, ActionName("Password")]
        public async Task<IActionResult> PasswordConfirmed(PasswordViewModel pvm)
        {
            var pwd = new PasswordHelper();
            var u = await PollContext.Users.FindAsync(pvm.UserId);
            if (u == null) { throw HttpException.NotFound; }

            if (pwd.IsPasswordValid(u, pvm.Password))
            {
                return RedirectToAction("Polls", "Admin", new { UserId = u.Id });
            }
            else { return View(); }
        }

        [HttpGet]
        public IActionResult Create(string login)
        {
            var user = new NewUserViewModel() { Login = login };
            return View(user);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateConfirmed(NewUserViewModel user)
        {
            if (user.Password == user.PasswordConfirmation)
            {
                var passwordHash = new PasswordHelper().GetHash(user.Password);
                var u = new User()
                {
                    Login = user.Login,
                    PasswordHash = passwordHash
                };

                var r = PollContext.Users.Add(u);
                await PollContext.SaveChangesAsync();

                return RedirectToAction("Polls", "Admin", new { UserId = r.Entity.Id });

            }
            else { return RedirectToAction("Create", new { user.Login }); }
        }

        #endregion Methods
    }
}