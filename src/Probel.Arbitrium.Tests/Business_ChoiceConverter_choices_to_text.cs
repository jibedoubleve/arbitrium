using Probel.Arbitrium.Business;
using Probel.Arbitrium.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Probel.Arbitrium.Tests
{
    public class Business_ChoiceConverter_choices_to_text
    {
        #region Fields

        private readonly string NL = Environment.NewLine;

        #endregion Fields
        [Fact]
        public void Returns_text_when_has_multiple_not_default_separator()
        {
            var choices = new List<Choice>
            {
                new Choice() { Text = "1" },
                new Choice() { Text = "2" },
                new Choice() { Text = "3" }
            };
            var poll = new Poll() { Choices = choices };

            var converter = new ChoiceConverter(poll, separator: "_");

            var result = converter.GetTextFromChoices();
            Assert.Equal($"1_2_3", result);
        }
        [Fact]
        public void Returns_text_when_has_multiple_default_separator()
        {
            var choices = new List<Choice>
            {
                new Choice() { Text = "1" },
                new Choice() { Text = "2" },
                new Choice() { Text = "3" }
            };
            var poll = new Poll() { Choices = choices };

            var converter = new ChoiceConverter(poll);

            var result = converter.GetTextFromChoices();
            Assert.Equal($"1{NL}2{NL}3", result);
        }
    }
}
