using Probel.Arbitrium.Business;
using Probel.Arbitrium.Models;
using System;
using System.Linq;
using Xunit;

namespace Probel.Arbitrium.Tests
{
    public class Business_ChoiceConverter_text_to_choices
    {
        #region Fields

        private readonly string NL = Environment.NewLine;

        #endregion Fields

        #region Methods

        [Fact]
        public void Returns_elements_when_has_multiple_not_default_separator()
        {
            var text = $"_Un _deux _trois{NL}_Quatre _cinq six_{NL}_Sept _Huit _Neuf{NL}_Dix _onze _douze";
            var converter = new ChoiceConverter(new Poll(), separator: "_");

            var result = converter.GetChoices(text);

            Assert.Equal(12, result.Count());
        }

        [Fact]
        public void Returns_empty_list_on_empty_text()
        {
            var converter = new ChoiceConverter(new Poll());

            var result = converter.GetChoices(string.Empty);

            Assert.Empty(result);
        }

        [Fact]
        public void Returns_multiple_elements_when_multiple_separators()
        {
            var text = $"*Un *deux *trois{NL}*Quatre *cinq six*{NL}*Sept *Huit *Neuf{NL}*Dix *onze *douze";
            var converter = new ChoiceConverter(new Poll());

            var result = converter.GetChoices(text);

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void Returns_one_element_when_no_separators()
        {
            var text = $"Un deux trois*Quatre cinq six*Sept Huit Neuf*Dix onze douze";
            var converter = new ChoiceConverter(new Poll());

            var result = converter.GetChoices(text);

            Assert.Single(result);
        }

        [Fact]
        public void Returns_zero_element_when_no_separator()
        {
            var text = $"*Un *deux *trois{NL}*Quatre *cinq six*{NL}*Sept *Huit *Neuf{NL}*Dix *onze *douze";
            var converter = new ChoiceConverter(new Poll(), separator: "_");

            var result = converter.GetChoices(text);

            Assert.Single(result);
        }

        #endregion Methods
    }
}