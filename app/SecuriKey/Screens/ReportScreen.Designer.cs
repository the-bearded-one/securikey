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
            newScanButton = new Controls.RoundedButton();
            reportButton = new Controls.RoundedButton();
            scanResultsText = new Label();
            resultsPanel = new TableLayoutPanel();
            openReportCheckBox = new CheckBox();
            SuspendLayout();
            // 
            // newScanButton
            // 
            newScanButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            newScanButton.BackColor = Color.Black;
            newScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            newScanButton.BorderRadius = 20;
            newScanButton.BorderThickness = 5F;
            newScanButton.ButtonText = "New Scan";
            newScanButton.ForeColor = Color.White;
            newScanButton.Location = new Point(571, 502);
            newScanButton.Margin = new Padding(3, 5, 3, 5);
            newScanButton.Name = "newScanButton";
            newScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            newScanButton.Size = new Size(212, 70);
            newScanButton.TabIndex = 1;
            newScanButton.UnpressedColor = Color.Black;
            // 
            // reportButton
            // 
            reportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            reportButton.BackColor = Color.Black;
            reportButton.BorderColor = Color.FromArgb(0, 66, 114);
            reportButton.BorderRadius = 20;
            reportButton.BorderThickness = 5F;
            reportButton.ButtonText = "Generate Report";
            reportButton.ForeColor = Color.White;
            reportButton.Location = new Point(15, 502);
            reportButton.Margin = new Padding(3, 5, 3, 5);
            reportButton.Name = "reportButton";
            reportButton.PressedColor = Color.FromArgb(0, 96, 166);
            reportButton.Size = new Size(212, 70);
            reportButton.TabIndex = 2;
            reportButton.UnpressedColor = Color.Black;
            // 
            // scanResultsText
            // 
            scanResultsText.Anchor = AnchorStyles.Top;
            scanResultsText.AutoSize = true;
            scanResultsText.Font = new Font("Arial", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            scanResultsText.ForeColor = Color.White;
            scanResultsText.Location = new Point(320, 12);
            scanResultsText.Name = "scanResultsText";
            scanResultsText.Size = new Size(176, 32);
            scanResultsText.TabIndex = 3;
            scanResultsText.Text = "Scan Results";
            // 
            // resultsPanel
            // 
            resultsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultsPanel.AutoScroll = true;
            resultsPanel.BackColor = Color.FromArgb(40, 40, 40);
            resultsPanel.ColumnCount = 1;
            resultsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            resultsPanel.Location = new Point(15, 61);
            resultsPanel.Name = "resultsPanel";
            resultsPanel.Padding = new Padding(10, 0, 10, 0);
            resultsPanel.RowCount = 1;
            resultsPanel.RowStyles.Add(new RowStyle());
            resultsPanel.Size = new Size(768, 433);
            resultsPanel.TabIndex = 4;
            // 
            // openReportCheckBox
            // 
            openReportCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            openReportCheckBox.AutoSize = true;
            openReportCheckBox.Checked = true;
            openReportCheckBox.CheckState = CheckState.Checked;
            openReportCheckBox.ForeColor = Color.White;
            openReportCheckBox.Location = new Point(233, 530);
            openReportCheckBox.Name = "openReportCheckBox";
            openReportCheckBox.Size = new Size(198, 24);
            openReportCheckBox.TabIndex = 5;
            openReportCheckBox.Text = "Open Report After Saving";
            openReportCheckBox.UseVisualStyleBackColor = true;
            // 
            // ReportScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(openReportCheckBox);
            Controls.Add(resultsPanel);
            Controls.Add(scanResultsText);
            Controls.Add(reportButton);
            Controls.Add(newScanButton);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ReportScreen";
            Size = new Size(800, 600);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Controls.RoundedButton newScanButton;
        private Controls.RoundedButton reportButton;
        private Label scanResultsText;
        private TableLayoutPanel resultsPanel;
        private CheckBox openReportCheckBox;
    }
}
