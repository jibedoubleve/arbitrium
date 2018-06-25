using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Business;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Admin;
using Probel.Arbitrium.ViewModels.Polls;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    public class AdminController : Controller
    {
        #region Fields

        private readonly AuthenticationHelper _auth;
        private readonly PollContext PollContext;

        #endregion Fields

        #region Constructors

        public AdminController(PollContext pollContext, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _auth = new AuthenticationHelper(userManager, contextAccessor);

            PollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        [HttpGet, ActionName("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePoll() => View(new RawPollViewModel() { UserId = await _auth.GetUserId() });

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePollConfirmed(RawPollViewModel vm)
        {
            var poll = new Poll();
            poll.Question = vm.Question;
            poll.Choices = new ChoiceConverter(poll).GetChoices(vm.Choices);

            PollContext.Polls.Add(poll);
            await PollContext.SaveChangesAsync();

            return RedirectToAction("List", "Poll");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePollConfirmed(long pollId)
        {
            var poll = await PollContext.Polls.FindAsync(pollId);

            if (poll == null) { throw new EntityNotFoundException(typeof(Poll), pollId); }
            else
            {
                PollContext.Polls.Remove(poll);
                await PollContext.SaveChangesAsync();
            }

            return RedirectToAction("Polls");
        }

        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditPoll(long pollId)
        {
            var poll = await (from p in PollContext.Polls.Include(e => e.Choices)
                              where p.Id == pollId
                              select p).FirstOrDefaultAsync();
            var converter = new ChoiceConverter(poll);

            if (poll == null) { throw new EntityNotFoundException(typeof(Poll), poll.Id); }
            else
            {
                return View(new RawPollViewModel()
                {
                    Choices = converter.GetTextFromChoices(),
                    Question = poll.Question,
                    UserId = await _auth.GetUserId(),
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPolls()
        {
            var query = new QueryPolls(PollContext);
            var result = await query.GetEditablePollsAsync();
            var vm = new PollCollectionViewModel()
            {
                NewPolls = result,
            };
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }

        #endregion Methods
    }
}