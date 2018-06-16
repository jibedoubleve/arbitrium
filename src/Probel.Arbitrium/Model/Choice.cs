namespace Probel.Arbitrium.Model
{
    public class Choice : Model
    {
        #region Properties

        public Poll Questionnaire { get; set; }
        public string Text { get; set; }

        #endregion Properties
    }
}