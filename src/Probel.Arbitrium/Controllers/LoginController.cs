using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.1&tabs=visual-studio%2Caspnetcore2x
    /// </remarks>
    public class LoginController : Controller
    {
        #region Fields

        private readonly PollContext PollContext;
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;

        private readonly ILogService Log = new LogService();

        #endregion Fields

        #region Constructors

        public LoginController(PollContext pollContext, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            PollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        [HttpPost]
        public async Task<IActionResult> Account(LoginViewModel user)
        {
            var result = await SignInManager.PasswordSignInAsync(user.Login,
                                                                 user.Password,
                                                                 true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var u = await UserManager.FindByEmailAsync(user.Login);
                return RedirectToAction("Polls", "Admin", new { UserId = u.Id });
            }
            else { throw HttpException.Unauthorized; }
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new NewLoginViewModel());
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateConfirmed(NewLoginViewModel newUser)
        {
            if (newUser.Password == newUser.PasswordConfirmation)
            {
                var user = new User() { Email = newUser.Login, UserName = newUser.Login };
                var result = await UserManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Polls", "Admin");
                }
                else { throw HttpException.InternalServerError; }
            }
            else { return View(new NewLoginViewModel()); }
        }

        #endregion Methods
    }
}