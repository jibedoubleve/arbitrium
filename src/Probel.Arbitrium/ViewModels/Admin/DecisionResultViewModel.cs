using System.Diagnostics;

namespace Probel.Arbitrium.ViewModels.Admin
{
    [DebuggerDisplay("IsYour: {IsYour} - Count: {Count}/{TotalCount} - Text: {Text}")]
    public class DecisionResultViewModel
    {
        #region Properties

        public int Count { get; set; }

        public bool IsYour { get; set; }

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