using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Probel.Arbitrium.Business;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Login;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.1&tabs=visual-studio%2Caspnetcore2x </remarks>
    public class LoginController : Controller
    {
        #region Fields

        private readonly IConfigurationService AppConfig;
        private readonly IAuthService Auth;
        private readonly PollContext PollContext;
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;

        #endregion Fields

        #region Constructors

        public LoginController(PollContext pollContext
            , SignInManager<User> signInManager
            , UserManager<User> userManager
            , IAuthService auth
            , IConfigurationService appConfig)
        {
            Auth = auth;
            AppConfig = appConfig;
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
                Name = RoleList.Admin,
                NormalizedName = RoleList.Admin.ToUpper()
            };
            var superUserRole = new IdentityRole<long>
            {
                Id = 2,
                Name = RoleList.SuperUser,
                NormalizedName = RoleList.SuperUser.ToUpper()
            };

            await roleStore.CreateAsync(adminRole);
            await roleStore.CreateAsync(superUserRole);

            await UserManager.AddToRoleAsync(user, adminRole.Name);
            await UserManager.AddToRoleAsync(user, superUserRole.Name);
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Connect()
        {
            if (await Auth.HasConnectedUser())
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                var cfg = await AppConfig.GetFullConfiguration();

                return View(new LoginViewModel { Configuration = cfg });
            }
            else { return RedirectToAction("ListNewPolls", "Poll"); }
        }

        [HttpPost, ActionName("Connect"), AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConnectConfirmed(LoginViewModel user)
        {
            if (user == null) { throw new ArgumentException(nameof(user)); }
            else if (string.IsNullOrEmpty(user.Login)) { ViewData["Message"] = "Login vide!"; }
            else if (string.IsNullOrEmpty(user.Password)) { ViewData["Message"] = "Mot de passe vide!"; }

            if (ViewData["Message"] != null) { return View(user); }

            var result = await SignInManager.PasswordSignInAsync(user.Login,
                                                                 user.Password,
                                                                 true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var u = await UserManager.FindByEmailAsync(user.Login);
                return RedirectToAction("ListNewPolls", "Poll");
            }
            else
            {
                ViewData["Message"] = "Mot de passe incorrect";
                return View(user);
            }
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Create() => View(new NewLoginViewModel());

        [HttpPost, ActionName("Create"), AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(NewLoginViewModel newUser)
        {
            if (newUser.Password == newUser.PasswordConfirmation)
            {
                var u = new UserHelper();
                var roleStore = new RoleStore<IdentityRole<long>, PollContext, long>(PollContext);
                var firstUser = !PollContext.Users.Any();

                var user = new User() { Email = u.RandomiseEmail(), UserName = newUser.UserName, SecurityStamp = Guid.NewGuid().ToString() };
                if (firstUser) { user.Id = 1; }

                var result = await UserManager.CreateAsync(user, newUser.Password);
                if (firstUser) { await AddFirstUser(user, roleStore); }

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ListNewPolls", "Poll");
                }
                else { throw new IdentityException(result.Errors); }
            }
            else
            {
                ViewData["Message"] = "Le mot de passe et la onfirmation ne correspondent pas!";
                return View(new NewLoginViewModel()); }
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Disconnect()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Connect", "Login");
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await Auth.GetConnectedUserAsync();
            return View(new NewLoginViewModel
            {
                UserName = user.UserName
            });
        }

        [HttpPost, ActionName("Edit"), Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(NewLoginViewModel model)
        {
            if (model.Password == model.PasswordConfirmation)
            {
                var user = await Auth.GetConnectedUserAsync();
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await UserManager.ResetPasswordAsync(user, token, model.Password);
                return RedirectToAction("List", "Poll");
            }
            else
            {
                ViewData["Message"] = "Le mot de passe et la confirmation ne correspondent pas!";
                return View(model);
            }
        }

        #endregion Methods
    }
}