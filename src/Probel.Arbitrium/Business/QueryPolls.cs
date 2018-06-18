using Microsoft.EntityFrameworkCore;
using Probel.Arbitrium.Core.Exception;
using Probel.Arbitrium.Models;
using Probel.Arbitrium.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Arbitrium.Business
{
    public class QueryPolls
    {
        #region Fields

        private readonly PollContext Context;

        #endregion Fields

        #region Constructors

        public QueryPolls(PollContext context)
        {
            Context = context;
        }

        #endregion Constructors

        #region Methods

        private async Task<int> GetCount(long userId, long choiceId)
        {
            return await (from d in Context.Decisions.Include(e => e.Choice)
                          where d.Choice.Id == choiceId
                          select d).CountAsync();
        }

        private async Task<int> GetTotalCount(long pollId)
        {
            return await (from d in Context.Decisions.Include(e => e.Choice)
                          where d.Choice.Poll.Id == pollId
                          select d).CountAsync();
        }

        private async Task<bool> IsYourDecision(long userId, long choiceId)
        {
            return await (from d in Context.Decisions.Include(e => e.Choice)
                          where d.User.Id == userId
                          && d.Choice.Id == choiceId
                          select d).CountAsync() > 0;
        }

        public async Task<IEnumerable<Poll>> GetNewPollsAsync(long userId)
        {
            var polls = await (from p in Context.Polls.Include(p => p.Choices).ThenInclude(c => c.Decisions)
                               where 0 == p.Choices.Where(c => c.Decisions.Where(d => d.User.Id == userId).Count() > 0).Count()
                               select p).ToListAsync();

            return polls;
        }

        public async Task<IEnumerable<Poll>> GetOldPollsAsync(long userId)
        {
            var polls = await (from p in Context.Polls.Include(p => p.Choices).ThenInclude(c => c.Decisions)
                               where 0 != p.Choices.Where(c => c.Decisions.Where(d => d.User.Id == userId).Count() > 0).Count()
                               select p).ToListAsync();

            return polls;
        }

        public async Task<PollResultViewModel> GetResultAsync(long userId, long pollId)
        {
            var poll = await (from p in Context.Polls.Include(e => e.Choices)
                              where p.Id == pollId
                              select p).SingleOrDefaultAsync();
            if (poll == null) { throw HttpException.NotFound; }
            else
            {
                var r = new PollResultViewModel();
                r.Question = poll.Question;

                var choices = new List<DecisionResultViewModel>();
                foreach (var choice in poll.Choices)
                {
                    choices.Add(new DecisionResultViewModel
                    {
                        Text = choice.Text,
                        Count = await GetCount(userId, choice.Id),
                        TotalCount = await GetTotalCount(poll.Id),
                        IsYour = await IsYourDecision(userId, choice.Id),
                    });
                }
                r.Choices = choices;
                r.UserId = userId;
                return r;
            }
        }

        public async Task<IEnumerable<PollResultViewModel>> GetAllResultsAsync(long userId)
        {
            var polls = await Context.Polls.Include(e => e.Choices).ThenInclude(f => f.Decisions).ToListAsync();
            var result = new List<PollResultViewModel>();

            foreach (var poll in polls)
            {
                var r = new PollResultViewModel();
                r.Question = poll.Question;

                var choices = new List<DecisionResultViewModel>();
                foreach (var choice in poll.Choices)
                {
                    choices.Add(new DecisionResultViewModel
                    {
                        Text = choice.Text,
                        Count = await GetCount(userId, choice.Id),
                        TotalCount = await GetTotalCount(poll.Id),
                        IsYour = await IsYourDecision(userId, choice.Id),
                    });
                }
                r.Choices = choices;
                result.Add(r);
            }

            return result;
        }

        public async Task<IEnumerable<Poll>> GetEditablePollsAsync()
        {
            var polls = await (from p in Context.Polls.Include(p => p.Choices).ThenInclude(c => c.Decisions)
                               where (p.Choices.Where(f => f.Decisions.Count() > 0).Count() == 0)
                               select p).ToListAsync();
            return polls;
        }

        #endregion Methods
    }
}