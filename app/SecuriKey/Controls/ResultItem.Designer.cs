using SecuriKey.Screens;

namespace SecuriKey.Controls
{
    partial class ResultItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            shortDescriptionLabel = new Label();
            scanTypeLabel = new Label();
            severityIndicator = new SeverityIndicator();
            detailsTextbox = new TextBox();
            aiHelpButton = new RoundedButton();
            arrowPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)arrowPictureBox).BeginInit();
            SuspendLayout();
            // 
            // shortDescriptionLabel
            // 
            shortDescriptionLabel.AutoSize = true;
            shortDescriptionLabel.BackColor = Color.Transparent;
            shortDescriptionLabel.Font = new Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            shortDescriptionLabel.ForeColor = Color.White;
            shortDescriptionLabel.Location = new Point(263, 11);
            shortDescriptionLabel.Name = "shortDescriptionLabel";
            shortDescriptionLabel.Size = new Size(129, 18);
            shortDescriptionLabel.TabIndex = 0;
            shortDescriptionLabel.Text = "short description";
            shortDescriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // scanTypeLabel
            // 
            scanTypeLabel.BackColor = Color.Transparent;
            scanTypeLabel.Font = new Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            scanTypeLabel.ForeColor = Color.White;
            scanTypeLabel.Location = new Point(30, 11);
            scanTypeLabel.Name = "scanTypeLabel";
            scanTypeLabel.Size = new Size(227, 18);
            scanTypeLabel.TabIndex = 1;
            scanTypeLabel.Text = "scan type";
            scanTypeLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // severityIndicator
            // 
            severityIndicator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            severityIndicator.BackColor = Color.Transparent;
            severityIndicator.BorderRadius = 8;
            severityIndicator.CriticalColor = Color.FromArgb(255, 0, 0);
            severityIndicator.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            severityIndicator.HighColor = Color.FromArgb(168, 37, 33);
            severityIndicator.Location = new Point(577, 6);
            severityIndicator.LowColor = Color.FromArgb(135, 133, 156);
            severityIndicator.MediumColor = Color.FromArgb(209, 130, 36);
            severityIndicator.Name = "severityIndicator";
            severityIndicator.OkColor = Color.FromArgb(61, 131, 97);
            severityIndicator.Severity = BusinessLogic.Severity.Low;
            severityIndicator.Size = new Size(83, 26);
            severityIndicator.TabIndex = 2;
            // 
            // detailsTextbox
            // 
            detailsTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            detailsTextbox.BackColor = Color.FromArgb(35, 36, 49);
            detailsTextbox.BorderStyle = BorderStyle.None;
            detailsTextbox.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            detailsTextbox.ForeColor = Color.White;
            detailsTextbox.Location = new Point(-1, 38);
            detailsTextbox.Multiline = true;
            detailsTextbox.Name = "detailsTextbox";
            detailsTextbox.ReadOnly = true;
            detailsTextbox.Size = new Size(763, 83);
            detailsTextbox.TabIndex = 3;
            detailsTextbox.Text = "details textbox";
            // 
            // aiHelpButton
            // 
            aiHelpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            aiHelpButton.BackColor = Color.Transparent;
            aiHelpButton.BorderColor = Color.FromArgb(30, 48, 243);
            aiHelpButton.BorderRadius = 8;
            aiHelpButton.BorderThickness = 4F;
            aiHelpButton.ButtonText = "AI Help";
            aiHelpButton.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            aiHelpButton.ForeColor = Color.White;
            aiHelpButton.Location = new Point(669, 6);
            aiHelpButton.Margin = new Padding(5, 3, 5, 3);
            aiHelpButton.Name = "aiHelpButton";
            aiHelpButton.PressedColor = Color.FromArgb(0, 96, 166);
            aiHelpButton.Size = new Size(83, 26);
            aiHelpButton.TabIndex = 4;
            aiHelpButton.UnpressedColor = Color.FromArgb(30, 48, 243);
            aiHelpButton.Click += OnAiHelpButtonClick;
            // 
            // arrowPictureBox
            // 
            arrowPictureBox.Image = Resources.Resources.upArrow;
            arrowPictureBox.Location = new Point(6, 13);
            arrowPictureBox.Name = "arrowPictureBox";
            arrowPictureBox.Size = new Size(13, 13);
            arrowPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            arrowPictureBox.TabIndex = 5;
            arrowPictureBox.TabStop = false;
            // 
            // ResultItem
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(arrowPictureBox);
            Controls.Add(detailsTextbox);
            Controls.Add(severityIndicator);
            Controls.Add(scanTypeLabel);
            Controls.Add(shortDescriptionLabel);
            Controls.Add(aiHelpButton);
            Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "ResultItem";
            Size = new Size(761, 38);
            Click += OnResultItemClick;
            MouseDown += OnResultItemMouseDown;
            MouseEnter += OnResultItemMouseEnter;
            MouseLeave += OnResultItemMouseLeave;
            MouseUp += OnResultItemMouseUp;
            Resize += OnResultItemResize;
            ((System.ComponentModel.ISupportInitialize)arrowPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label shortDescriptionLabel;
        private Label scanTypeLabel;
        private SeverityIndicator severityIndicator;
        private TextBox detailsTextbox;
        private RoundedButton aiHelpButton;
        private PictureBox arrowPictureBox;
    }
}
