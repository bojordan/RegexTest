
namespace RegexTest
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    public partial class RegexTestForm : Form
    {
        private Color _originalColor;

        private Color _originalBackColor;

        private Color _originalBackColor2;

        public RegexTestForm()
        {
            InitializeComponent();
            _originalColor = this.stringTextBox.SelectionColor;
            _originalBackColor = this.stringTextBox.SelectionBackColor;
            _originalBackColor2 = this.resultsTextBox.SelectionBackColor;
        }

        private readonly Color[] _colors = new []
            {
                Color.FromArgb(255, 142, 183, 197), // blue 1
                Color.FromArgb(255, 175, 204, 214), // blue 2
                Color.FromArgb(255, 209, 226, 232) // blue 3
            };

        private struct MatchFormatting
        {
            public int Index;

            public int Length;

            public Color Color;
        }

        private void EvaluateMatches(object sender, EventArgs args)
        {
            int selectionStart = this.stringTextBox.SelectionStart;
            List<MatchFormatting> matchFormatters = new List<MatchFormatting>();
            try
            {
                var matches = Regex.Matches(this.stringTextBox.Text, this.patternTextBox.Text);

                this.stringTextBox.SelectAll();
                this.stringTextBox.SelectionColor = _originalColor;
                this.stringTextBox.SelectionBackColor = _originalBackColor;

                this.resultsTextBox.SelectAll();
                this.resultsTextBox.SelectionBackColor = _originalBackColor2;

                this.resultsTextBox.Text = string.Empty;
                string matchOutput = "match {0} @{1}:{2} ";// " {3}";
                string groupOutput = "  group {0} @{1}:{2} ";// " {3}";
                StringBuilder sb = new StringBuilder();
                int numberOfNewlines = 0;
                for (int i = 0; i < matches.Count; i++)
                {
                    sb.Append(string.Format(matchOutput, i, matches[i].Index, matches[i].Length));
                    MatchFormatting formatting = new MatchFormatting();
                    formatting.Index = sb.Length;
                    formatting.Length = matches[i].ToString().Length;
                    formatting.Color = _colors[0];
                    matchFormatters.Add(formatting);
                    sb.AppendLine(matches[i].ToString());
                    numberOfNewlines++;

                    if (matches[i].Length > 0)
                    {
                        this.stringTextBox.Select(matches[i].Index, matches[i].Length);
                        this.stringTextBox.SelectionBackColor = _colors[0];

                        if (matches[i].Groups.Count > 1)
                        {
                            sb.AppendLine("{");
                            numberOfNewlines++;
                        }

                        for (int j = 1; j < matches[i].Groups.Count; j++)
                        {
                            sb.Append(
                                string.Format(
                                    groupOutput,
                                    j,
                                    matches[i].Groups[j].Index,
                                    matches[i].Groups[j].Length));

                            this.stringTextBox.Select(matches[i].Groups[j].Index, matches[i].Groups[j].Length);
                            this.stringTextBox.SelectionBackColor = _colors[1];

                            string groupMatch = matches[i].Groups[j].ToString();

                            MatchFormatting groupFormatting = new MatchFormatting();
                            groupFormatting.Index = sb.Length - numberOfNewlines;
                            groupFormatting.Length = groupMatch.Length;
                            groupFormatting.Color = _colors[1];
                            matchFormatters.Add(groupFormatting);

                            sb.AppendLine(groupMatch);
                            numberOfNewlines++;
                        }

                        if (matches[i].Groups.Count > 1)
                        {
                            sb.AppendLine("}");
                            numberOfNewlines++;
                        }
                    }
                }

                this.resultsTextBox.Text = sb.ToString();
                foreach (var formatter in matchFormatters)
                {
                    this.resultsTextBox.Select(formatter.Index, formatter.Length);
                    this.resultsTextBox.SelectionBackColor = formatter.Color;
                }
            }
            catch (Exception ex)
            {
                this.resultsTextBox.Text = ex.Message;
            }
            finally
            {
                this.stringTextBox.Select(selectionStart, 0);
                this.stringTextBox.SelectionColor = _originalColor;
                this.stringTextBox.SelectionBackColor = _originalBackColor;

                this.resultsTextBox.Select(0, 0);
                this.resultsTextBox.SelectionBackColor = _originalBackColor2;
            }
        }
    }
}
