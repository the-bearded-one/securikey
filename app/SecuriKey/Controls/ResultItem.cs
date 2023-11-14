using BusinessLogic;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace SecuriKey.Controls
{
    public partial class ResultItem : UserControl
    {
        int squashedHeight = 0;
        Color mouseOverHighlightColor = Color.FromArgb(109, 158, 235);
        Color mouseDownHighlightColor = Color.FromArgb(45, 65, 97);
        Color idleColor = Color.FromArgb(0, 0, 0);

        #region Constructors


        public ResultItem(SecurityCheck result) : this()
        {
            this.GptPrompt = result.AiAssistantPrompt;
            this.RiskName = result.Name;
            switch (result.Severity.Rating)
            {
                case Severities.CRITICAL:
                    this.Severity = Severity.Critical;
                    break;
                case Severities.HIGH:
                    this.Severity = Severity.High;
                    break;
                case Severities.MEDIUM:
                    this.Severity = Severity.Medium;
                    break;
                case Severities.LOW:
                    this.Severity = Severity.Low;
                    break;
            }
            this.ScanType = result.Name;
            this.ShortDescription = result.Relevance;
            if ( this.ShortDescription.Length > 180 )
            {
                this.ShortDescription = this.ShortDescription.Substring(0, 180) + "...";
            }
            this.Details = result.Severity.Justification;
        }

        public ResultItem()
        {
            InitializeComponent();

            // remember our starting height
            squashedHeight = this.Height;

            // remember idel background color
            idleColor = this.BackColor;

            GptPrompt = String.Empty;
            ID = String.Empty;
            RiskName = String.Empty;

            // bubble up mouse events to this control
            scanTypeLabel.Click += OnResultItemClick;
            shortDescriptionLabel.Click += OnResultItemClick;
            severityIndicator.Click += OnResultItemClick;

            shortDescriptionLabel.MouseLeave += OnResultItemMouseLeave;
            severityIndicator.MouseLeave += OnResultItemMouseLeave;
            detailsTextbox.MouseLeave += OnResultItemMouseLeave;
            aiHelpButton.MouseLeave += OnResultItemMouseLeave;
        }
        #endregion

        #region Properties
        private bool IsSquashed { get; set; } = true;
        public Severity Severity
        {
            get => severityIndicator.Severity;
            set => severityIndicator.Severity = value;
        }
        public string ScanType
        {
            get => scanTypeLabel.Text;
            set => scanTypeLabel.Text = value;
        }
        public string ShortDescription
        {
            get => shortDescriptionLabel.Text;
            set => shortDescriptionLabel.Text = value;
        }
        public string Details
        {
            get => detailsTextbox.Text;
            set => detailsTextbox.Text = value;
        }

        public String GptPrompt { get; private set; }
        public String ID { get; private set; }
        
        public String RiskName { get; private set; }

        #endregion

        #region Event Handlers
        private void OnResultItemClick(object sender, EventArgs e)
        {
            IsSquashed = !IsSquashed;

            if (IsSquashed) this.Height = squashedHeight;
            else this.Height = detailsTextbox.Bottom + 1;
            Invalidate();
        }

        private void OnResultItemMouseEnter(object sender, EventArgs e)
        {
            this.BackColor = mouseOverHighlightColor;
            this.Invalidate();
        }

        private void OnResultItemMouseLeave(object sender, EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                this.BackColor = mouseOverHighlightColor;
            }
            else
            {
                this.BackColor = idleColor;
            }
            this.Invalidate();
        }

        private void OnResultItemMouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = mouseDownHighlightColor;
            this.Invalidate();
        }

        private void OnResultItemMouseUp(object sender, MouseEventArgs e)
        {
            if (this.ClientRectangle.Contains(e.Location))
            {
                this.BackColor = mouseOverHighlightColor;
            }
            else
            {
                this.BackColor = idleColor;
            }
            this.Invalidate();
        }
        #endregion
    }
}
