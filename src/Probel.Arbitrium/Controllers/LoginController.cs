using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Login;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.1&tabs=visual-studio%2Caspnetcore2x </remarks>
    public class LoginController : Controller
    {
        #region Fields

        private readonly PollContext PollContext;
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;

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

        private async Task AddFirstUser(User user, RoleStore<IdentityRole<long>, PollContext, long> roleStore)
        {
            var adminRole = new IdentityRole<long>
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
            var userRole = new IdentityRole<long>
            {
                Id = 2,
                Name = "User",
                NormalizedName = "USER"
            };

            await roleStore.CreateAsync(userRole);
            await roleStore.CreateAsync(adminRole);

            await UserManager.AddToRoleAsync(user, adminRole.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Connect(LoginViewModel user)
        {
            var result = await SignInManager.PasswordSignInAsync(user.Login,
                                                                 user.Password,
                                                                 true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var u = await UserManager.FindByEmailAsync(user.Login);
                return RedirectToAction("List", "Poll");
            }
            else { throw new ConnectionException(result); }
        }

        [HttpGet]
        public async Task<IActionResult> Connect()
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
                var roleStore = new RoleStore<IdentityRole<long>, PollContext, long>(PollContext);
                var firstUser = !PollContext.Users.Any();

                var user = new User() { Email = newUser.Login, UserName = newUser.UserName };
                if (firstUser) { user.Id = 1; }

                var result = await UserManager.CreateAsync(user, newUser.Password);
                if (firstUser) { await AddFirstUser(user, roleStore); }

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("List", "Poll");
                }
                else { throw new IdentityException(result.Errors); }
            }
            else { return View(new NewLoginViewModel()); }
        }

        [HttpPost]
        public async Task<IActionResult> Disconnect()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Connect", "Login");
        }

        #endregion Methods
    }
}