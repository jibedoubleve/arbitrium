using Probel.Arbitrium.Constants;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Probel.Arbitrium.ViewModels.Admin
{
    [DebuggerDisplay("IsYour: {IsYour} - Count: {Count}/{TotalCount} - Text: {Text}")]
    public class DecisionResultViewModel
    {
        #region Properties

        public int Count { get; set; }

        public bool IsYour { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Format.Percent)]
        public decimal Percentage
        {
            get
            {
                if (TotalCount == 0) { return 0; }
                else { return (((decimal)Count / TotalCount)) * 100; }
            }
        }

        public string Text { get; set; }

        public int TotalCount { get; set; }

        #endregion Properties
    }
}