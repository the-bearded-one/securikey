namespace SecuriKey.Screens
{
    partial class ReportScreen
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
            statusTextbox = new TextBox();
            newScanButton = new Controls.RoundedButton();
            reportButton = new Controls.RoundedButton();
            scanResultsText = new Label();
            SuspendLayout();
            // 
            // statusTextbox
            // 
            statusTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusTextbox.BackColor = Color.Black;
            statusTextbox.BorderStyle = BorderStyle.FixedSingle;
            statusTextbox.ForeColor = Color.White;
            statusTextbox.Location = new Point(15, 76);
            statusTextbox.Margin = new Padding(3, 4, 3, 4);
            statusTextbox.Multiline = true;
            statusTextbox.Name = "statusTextbox";
            statusTextbox.ReadOnly = true;
            statusTextbox.ScrollBars = ScrollBars.Vertical;
            statusTextbox.Size = new Size(845, 533);
            statusTextbox.TabIndex = 0;
            // 
            // newScanButton
            // 
            newScanButton.BackColor = Color.Black;
            newScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            newScanButton.BorderRadius = 40;
            newScanButton.BorderThickness = 5F;
            newScanButton.ButtonText = "New Scan";
            newScanButton.ForeColor = Color.White;
            newScanButton.Location = new Point(493, 648);
            newScanButton.Margin = new Padding(3, 5, 3, 5);
            newScanButton.Name = "newScanButton";
            newScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            newScanButton.Size = new Size(368, 123);
            newScanButton.TabIndex = 1;
            // 
            // reportButton
            // 
            reportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            reportButton.BackColor = Color.Black;
            reportButton.BorderColor = Color.FromArgb(0, 66, 114);
            reportButton.BorderRadius = 40;
            reportButton.BorderThickness = 5F;
            reportButton.ButtonText = "Generate Report";
            reportButton.ForeColor = Color.White;
            reportButton.Location = new Point(15, 648);
            reportButton.Margin = new Padding(3, 5, 3, 5);
            reportButton.Name = "reportButton";
            reportButton.PressedColor = Color.FromArgb(0, 96, 166);
            reportButton.Size = new Size(368, 123);
            reportButton.TabIndex = 2;
            // 
            // scanResultsText
            // 
            scanResultsText.AutoSize = true;
            scanResultsText.Font = new Font("Arial", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            scanResultsText.ForeColor = Color.White;
            scanResultsText.Location = new Point(320, 12);
            scanResultsText.Name = "scanResultsText";
            scanResultsText.Size = new Size(176, 32);
            scanResultsText.TabIndex = 3;
            scanResultsText.Text = "Scan Results";
            // 
            // ReportScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(scanResultsText);
            Controls.Add(reportButton);
            Controls.Add(newScanButton);
            Controls.Add(statusTextbox);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ReportScreen";
            Size = new Size(878, 864);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox statusTextbox;
        private Controls.RoundedButton newScanButton;
        private Controls.RoundedButton reportButton;
        private Label scanResultsText;
    }
}
