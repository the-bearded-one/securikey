namespace SecuriKey.Screens
{
    partial class ScanningScreen
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
            progressBar = new ProgressBar();
            progressLabel = new Label();
            encouragementLabel = new Label();
            SuspendLayout();
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.None;
            progressBar.ForeColor = Color.FromArgb(0, 66, 114);
            progressBar.Location = new Point(35, 164);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(724, 36);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 0;
            // 
            // progressLabel
            // 
            progressLabel.Anchor = AnchorStyles.None;
            progressLabel.AutoSize = true;
            progressLabel.BackColor = Color.Black;
            progressLabel.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            progressLabel.ForeColor = Color.White;
            progressLabel.Location = new Point(35, 116);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new Size(131, 27);
            progressLabel.TabIndex = 1;
            progressLabel.Text = "scanning...";
            // 
            // encouragementLabel
            // 
            encouragementLabel.Anchor = AnchorStyles.None;
            encouragementLabel.BackColor = Color.Black;
            encouragementLabel.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            encouragementLabel.ForeColor = Color.White;
            encouragementLabel.Location = new Point(-39, 289);
            encouragementLabel.Name = "encouragementLabel";
            encouragementLabel.Size = new Size(874, 36);
            encouragementLabel.TabIndex = 2;
            encouragementLabel.Text = "nice!";
            encouragementLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ScanningScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(encouragementLabel);
            Controls.Add(progressLabel);
            Controls.Add(progressBar);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ScanningScreen";
            Size = new Size(800, 600);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar;
        private Label progressLabel;
        private Label encouragementLabel;
    }
}
