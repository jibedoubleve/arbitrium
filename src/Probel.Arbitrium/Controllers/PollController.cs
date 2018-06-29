﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Business;
using Probel.Arbitrium.Exceptions;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Polls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Controllers
{
    public class PollController : Controller
    {
        #region Fields

        private readonly IAuthService _auth;
        private readonly PollContext PollContext;

        #endregion Fields

        #region Constructors

        public PollController(PollContext pollContext, UserManager<User> userManager, IHttpContextAccessor contextAccessor, IAuthService auth)
        {
            _auth = auth;

            PollContext = pollContext;
        }

        #endregion Constructors

        #region Methods

        [HttpGet, Authorize]
        public async Task<IActionResult> ListResults()
        {
            var query = new QueryPolls(PollContext);
            var uid = await _auth.GetConnectedUserIdAsync();
            var newPolls = await query.GetNewPollsAsync(uid);
            var oldPolls = await query.GetOldPollsAsync(uid);

            var vm = new PollCollectionViewModel()
            {
                NewPolls = newPolls,
                OldPolls = oldPolls,
            };
            return View(vm);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> ListNewPolls()
        {
            var query = new QueryPolls(PollContext);
            var uid = await _auth.GetConnectedUserIdAsync();
            var newPolls = await query.GetNewPollsAsync(uid);

            var vm = new PollCollectionViewModel()
            {
                NewPolls = newPolls,
            };
            return View(vm);

        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Result(long pollId)
        {
            var userId = await _auth.GetConnectedUserIdAsync();
            var user = await PollContext.Users.FindAsync(userId);
            var poll = await PollContext.Polls.FindAsync(pollId);

            if (user == null) { throw new EntityNotFoundException(typeof(User), userId); }
            if (poll == null) { throw new EntityNotFoundException(typeof(Poll), pollId); }

            var pollResult = await new QueryPolls(PollContext).GetResultAsync(await _auth.GetConnectedUserIdAsync(), pollId);

            return View(pollResult);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Vote(long pollId)
        {
            var poll = await (from p in PollContext.Polls.Include(e => e.Choices)
                              where p.Id == pollId
                              select p).SingleOrDefaultAsync();

            if (poll == null) { throw new EntityNotFoundException(typeof(Poll), pollId); }
            return View(new VoteViewModel() { Poll = poll });
        }

        [HttpPost, ActionName("Vote"), Authorize]
        public async Task<IActionResult> VoteConfirmed(VoteViewModel vm)
        {
            var choiceId = vm.ChoiceId;

            if (choiceId == default(long)) { throw new ArgumentException("Unsupported value", nameof(choiceId)); }

            var userId = await _auth.GetConnectedUserIdAsync();
            var user = await PollContext.Users.FindAsync(userId);
            var choice = await PollContext.Choices.FindAsync(choiceId);

            if (user == null) { throw new EntityNotFoundException(typeof(User), userId); }
            if (choice == null) { throw new EntityNotFoundException(typeof(Choice), choiceId); }

            var decision = new Decision()
            {
                Choice = choice,
                Date = DateTime.Now.ToUniversalTime(),
                User = user,
            };
            PollContext.Decisions.Add(decision);
            await PollContext.SaveChangesAsync();

            return RedirectToAction("ListResult", "Poll");
        }

        #endregion Methods
    }
}