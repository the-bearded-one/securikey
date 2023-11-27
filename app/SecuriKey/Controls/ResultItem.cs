using BusinessLogic;
using Microsoft.VisualBasic.Devices;
using SecuriKey.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            this.Details = result.Severity.Justification;

            // hide ai button if online is not authorized
            if (!BL.Instance.IsInternetConnectionAuthorized)
            {
                aiHelpButton.Visible = false;
                severityIndicator.Location = aiHelpButton.Location;
            }

            RescaleTextToFit();
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
            arrowPictureBox.Click += OnResultItemClick;

            shortDescriptionLabel.MouseLeave += OnResultItemMouseLeave;
            severityIndicator.MouseLeave += OnResultItemMouseLeave;
            detailsTextbox.MouseLeave += OnResultItemMouseLeave;
            aiHelpButton.MouseLeave += OnResultItemMouseLeave;

            this.DoubleBuffered = true;
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
        public string ShortDescription = string.Empty;
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

            if (IsSquashed)
            {
                CollapseControl();
                this.arrowPictureBox.Image = Resources.Resources.upArrow;
            }
            else
            {
                ExpandControl();
                this.arrowPictureBox.Image = Resources.Resources.downArrow;
            }
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

        private void OnAiHelpButtonClick(object sender, EventArgs e)
        {
            // pass the ID and prompt to the assistant for the GPT API call
            AiAssistant aiAssistantForm = new AiAssistant
            {
                RiskName = this.RiskName,
                ID = this.ID,
                GptPrompt = this.GptPrompt
            };
            aiAssistantForm.StartPosition = FormStartPosition.CenterParent;
            aiAssistantForm.ShowDialog(this);
        }

        private void OnResultItemResize(object sender, EventArgs e)
        {
            RescaleTextToFit();
        }
        #endregion

        #region private methods
        private void ExpandControl()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { ExpandControl(); }));
            }
            else
            {
                while (this.Height < detailsTextbox.Bottom + 1)
                {
                    this.Height += 6;
                    Thread.Sleep(10);
                }
                this.Height = detailsTextbox.Bottom + 1;
            }
        }

        private void CollapseControl()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { CollapseControl(); }));
            }
            else
            {
                while (this.Height > squashedHeight)
                {
                    this.Height -= 6;
                    Thread.Sleep(10);
                }
                this.Height = squashedHeight;
            }
        }

        private void RescaleTextToFit()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { RescaleTextToFit(); }));
            }
            else
            {
                if (this.Visible)
                {
                    try
                    {
                        var resizedString = ShortDescription;
                        var allowedWidth = severityIndicator.Left - shortDescriptionLabel.Left;
                        shortDescriptionLabel.Text = resizedString;
                        while (shortDescriptionLabel.Width > allowedWidth)
                        {
                            var newLength = shortDescriptionLabel.Text.Length - 12;
                            shortDescriptionLabel.Text = $"{shortDescriptionLabel.Text.Substring(0, newLength)}...";
                        }
                    }
                    catch
                    {
                        // do nothing. just don't crash
                    }
                }
            }
            #endregion
        }
    }
}
