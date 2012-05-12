
namespace RegexTest
{
    using System;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    public partial class RegexTestForm : Form
    {
        public RegexTestForm()
        {
            InitializeComponent();
        }

        private void EvaluateMatches(object sender, EventArgs args)
        {
            try
            {
                var matches = Regex.Matches(this.stringTextBox.Text, this.patternTextBox.Text);

                this.resultsTextBox.Text = string.Empty;
                string matchOutput = "{0} @{1}:{2} {3}";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < matches.Count; i++)
                {
                    sb.AppendLine(string.Format(matchOutput, i, matches[i].Index, matches[i].Length, matches[i]));
                }

                this.resultsTextBox.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                this.resultsTextBox.Text = ex.Message;
            }
        }
    }
}
