﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Business;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Admin;
using Probel.Arbitrium.ViewModels.Login;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    public class AdminController : Controller
    {
        #region Fields

        private readonly IConfigurationService AppConfig;
        private readonly IAuthService Auth;
        private readonly PollContext PollContext;
        private readonly RoleManager<IdentityRole<long>> RoleManager;
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;

        #endregion Fields

        #region Constructors

        public AdminController(PollContext pollContext
            , UserManager<User> userManager
            , RoleManager<IdentityRole<long>> roleManager
            , SignInManager<User> signInManager
            , IHttpContextAccessor contextAccessor
            , IAuthService auth
            , IConfigurationService appConfig)
        {
            Auth = auth;
            AppConfig = appConfig;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;

            PollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        private async Task CreatePollAsync(RawPollViewModel vm)
        {
            var poll = new Poll
            {
                Question = vm.Question,
                StartDate = vm.StartDate.ToUniversalTime(),
                EndDate = vm.EndDate.ToUniversalTime(),
            };
            poll.Choices = new ChoiceConverter(poll).GetChoices(vm.Choices);

            PollContext.Polls.Add(poll);
            await PollContext.SaveChangesAsync();
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Configuration()
        {
            var v = await (from s in PollContext.Settings
                           where s.Key == ConfigKeys.RegistrationStatus
                           select s.Value).SingleOrDefaultAsync();
            var regEnabled = (v != null && v == "enabled");

            return View(new AppConfigurationViewModel { IsRegistrationEnabled = regEnabled });
        }

        [HttpPost, ActionName("Configuration"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfigurationConfirmed(AppConfigurationViewModel vm)
        {
            var value = vm.IsRegistrationEnabled ? ConfigValues.Enabled : ConfigValues.Disabled;
            await AppConfig.UpdateAsync(ConfigKeys.RegistrationStatus, value);

            ViewData["Message"] = "Configuration sauvée!";
            return View();
        }

        [HttpGet, Authorize(Roles = "Admin, SuperUser")]
        public IActionResult CreatePoll() => View(new RawPollViewModel());

        [HttpPost, ActionName("CreatePoll"), Authorize(Roles = "Admin, SuperUser")]
        public async Task<IActionResult> CreatePollConfirmed(RawPollViewModel vm)
        {
            await CreatePollAsync(vm);

            return RedirectToAction("ListNewPolls", "Poll");
        }

        [HttpPost, ActionName("DeletePoll"), Authorize(Roles = "Admin, SuperUser")]
        public async Task<IActionResult> DeletePollConfirmed(long id)
        {
            var poll = await PollContext.Polls.FindAsync(id);

            if (poll == null) { throw new EntityNotFoundException(typeof(Poll), id); }
            else
            {
                PollContext.Polls.Remove(poll);
                await PollContext.SaveChangesAsync();
            }

            return RedirectToAction("ListPolls");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            var user = await (from u in PollContext.Users
                              where u.Id == userId
                              select u).SingleOrDefaultAsync();

            PollContext.Users.Remove(user);
            await PollContext.SaveChangesAsync();

            return RedirectToAction("ListUsers");
        }

        [HttpGet, Authorize(Roles = "Admin, SuperUser")]
        public async Task<IActionResult> EditPoll(long id)
        {
            var poll = await (from p in PollContext.Polls.Include(e => e.Choices)
                              where p.Id == id
                              select p).FirstOrDefaultAsync();
            var converter = new ChoiceConverter(poll);

            if (poll == null) { throw new EntityNotFoundException(typeof(Poll), poll.Id); }
            else
            {
                return View(new RawPollViewModel()
                {
                    Choices = converter.GetTextFromChoices(),
                    Question = poll.Question,
                    PollId = poll.Id,
                    StartDate = poll.StartDateLocal,
                    EndDate = poll.EndDateLocal,
                });
            }
        }

        [HttpPost, ActionName("EditPoll"), Authorize(Roles = "Admin, SuperUser")]
        public async Task<IActionResult> EditPollConfirmed(RawPollViewModel model)
        {
            var poll = await PollContext.Polls.FindAsync(model.PollId);
            PollContext.Polls.Remove(poll);
            await CreatePollAsync(model);

            return RedirectToAction("ListPolls");
        }

        public IActionResult Error() => View();

        [HttpGet, Authorize(Roles = "Admin, SuperUser")]
        public async Task<IActionResult> ListPolls()
        {
            var query = new PollQueryService(PollContext);
            var editablePolls = await query.GetEditablePollsAsync();

            return View(editablePolls);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUsers()
        {
            var currentUser = await Auth.GetConnectedUserAsync();
            var users = await (from u in PollContext.Users
                               where u.UserName != currentUser.UserName
                               select u).ToListAsync();
            return View(new UserCollectionViewModel { RegisteredUsers = users });
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult UpdatePassword(long userId)
        {
            var user = PollContext.Users.Find(userId);
            return View(new NewLoginViewModel { UserName = user.UserName });
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ActionName("UpdatePassword")]
        public async Task<IActionResult> UpdatePasswordConfirmed(NewLoginViewModel model)
        {
            if (model.Password == model.PasswordConfirmation)
            {
                var user = await (from u in PollContext.Users
                                  where u.UserName == model.UserName
                                  select u).SingleAsync();

                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await UserManager.ResetPasswordAsync(user, token, model.Password);
                return RedirectToAction("ListUsers");
            }
            else
            {
                ViewData["Message"] = "Le mot de passe et la confirmation ne correspondent pas!";
                return View(model);
            }
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(long userId)
        {
            var user = PollContext.Users.Find(userId);
            var roles = await Auth.GetRolesAsStringAsync(userId);

            if (roles != null)
            {
                var vm = new UserRoleViewModel
                {
                    UserId = userId,
                    UserName = user.UserName,
                    UserRoles = roles.ToList()
                };
                return View(vm);
            }
            else { throw new EntityNotFoundException($"The user with id '{userId}' does not have roles!"); }
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ActionName("UpdateRole")]
        public async Task<IActionResult> UpdateRoleConfirmed(UserRoleViewModel vm)
        {
            var user = await PollContext.Users.FindAsync(vm.UserId);
            foreach (var role in vm.Roles)
            {
                await UserManager.RemoveFromRoleAsync(user, role);
            }

            foreach (var role in vm.UserRoles)
            {
                if (role.IsSelected) { await UserManager.AddToRoleAsync(user, role.Name); }
            }

            return RedirectToAction("UpdateRole", new { vm.UserId });
        }

        #endregion Methods
    }
}