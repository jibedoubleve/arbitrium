using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Business;
using Probel.Arbitrium.Core.Exception;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.Services;
using Probel.Arbitrium.ViewModels;
using Probel.Arbitrium.ViewModels.Admin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    public class AdminController : Controller
    {
        #region Fields

        private readonly PollContext PollContext;

        #endregion Fields

        #region Constructors

        public AdminController(PollContext pollContext)
        {
            PollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        [HttpGet]
        public IActionResult Create(long userId) => View(new RawPollViewModel() { UserId = userId });

        [HttpPost]
        public async Task<IActionResult> Create(RawPollViewModel vm)
        {
            var poll = new Poll();
            poll.Question = vm.Question;
            poll.Choices = new ChoiceConverter(poll).GetChoices(vm.Choices);

            PollContext.Polls.Add(poll);
            await PollContext.SaveChangesAsync();

            return RedirectToAction("Polls", new { vm.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long pollId, long userId)
        {
            var poll = await (from p in PollContext.Polls.Include(e => e.Choices)
                              where p.Id == pollId
                              select p).FirstOrDefaultAsync();
            var converter = new ChoiceConverter(poll);

            if (poll == null) { throw HttpException.NotFound; }
            else
            {
                return View(new RawPollViewModel()
                {
                    Choices = converter.GetTextFromChoices(),
                    Question = poll.Question,
                    UserId = userId,
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RawPollViewModel vm)
        {
            var poll = await PollContext.Polls.FindAsync(vm.PollId);
            return RedirectToAction("Polls", new { vm.UserId });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var poll = await PollContext.Polls.FindAsync(id);

            if (poll == null) { throw HttpException.NotFound; }
            else
            {
                PollContext.Polls.Remove(poll);
                await PollContext.SaveChangesAsync();
            }

            return RedirectToAction("Polls");
        }

        [HttpGet]
        public async Task<IActionResult> Details(long pollId, long userId)
        {
            var poll = await (from p in PollContext.Polls.Include(e => e.Choices)
                              where p.Id == pollId
                              select p).SingleOrDefaultAsync();

            if (poll == null) { throw HttpException.NotFound; }
            return View(new VoteViewModel() { Poll = poll, UserId = userId });
        }

        public string Error()
        {
            return $"An error occured!";
        }

        [HttpGet]
        public async Task<IActionResult> Polls(long userId)
        {
            var query = new QueryPolls(PollContext);
            var newPolls = await query.GetNewPollsAsync(userId);
            var oldPolls = await query.GetOldPollsAsync(userId);

            var vm = new PollCollectionViewModel()
            {
                NewPolls = newPolls,
                OldPolls = oldPolls,
                UserId = userId,
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPolls(long userId)
        {
            var query = new QueryPolls(PollContext);
            var result = await query.GetEditablePollsAsync();
            var vm = new PollCollectionViewModel()
            {
                NewPolls = result,
                UserId = userId
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Vote(long choiceId, long userId)
        {
            var user = await PollContext.Users.FindAsync(userId);
            var choice = await PollContext.Choices.FindAsync(choiceId);

            if (user == null) { throw HttpException.NotFound; }
            if (choice == null) { throw HttpException.NotFound; }

            var decision = new Decision()
            {
                Choice = choice,
                Date = DateTime.Now.ToUniversalTime(),
                User = user,
            };
            PollContext.Decisions.Add(decision);
            await PollContext.SaveChangesAsync();

            return RedirectToAction("Polls", new { UserId = userId });
        }

        [HttpGet]
        public async Task<IActionResult> Result(long pollId, long userId)
        {
            var user = await PollContext.Users.FindAsync(userId);
            var choice = await PollContext.Polls.FindAsync(pollId);

            if (user == null) { throw HttpException.NotFound; }
            if (choice == null) { throw HttpException.NotFound; }

            var pollResult = await new QueryPolls(PollContext).GetResultAsync(userId, pollId);

            return View(pollResult);
        }

        #endregion Methods
    }
}