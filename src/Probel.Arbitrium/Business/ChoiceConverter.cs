using Probel.Arbitrium.Models;
using System;
using System.Collections.Generic;

namespace Probel.Arbitrium.Business
{
    public class ChoiceConverter
    {
        #region Fields

        private readonly Poll _poll;

        private readonly string _separator = Environment.NewLine;

        #endregion Fields

        #region Constructors

        public ChoiceConverter(Poll poll, string separator = null)
        {
            _poll = poll;
            _separator = separator ?? Environment.NewLine;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<Choice> GetChoices(string rawData)
        {
            var result = new List<Choice>();
            var choices = GetChoicesAsString(rawData);
            foreach (var choice in choices)
            {
                result.Add(new Choice()
                {
                    Poll = _poll,
                    Text = choice,
                });
            }
            return result;
        }

        public string GetTextFromChoices()
        {
            var result = string.Empty;
            foreach (var choice in _poll.Choices)
            {
                result += choice.Text;
                result += _separator;
            }
            result = result.TrimEnd(_separator.ToCharArray());
            return result;
        }

        private IEnumerable<string> GetChoicesAsString(string rawData)
        {
            var r = rawData.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            return r;
        }

        #endregion Methods
    }
}