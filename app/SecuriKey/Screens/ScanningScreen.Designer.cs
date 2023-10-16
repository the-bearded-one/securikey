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
            progressBar.Anchor = AnchorStyles.Top;
            progressBar.ForeColor = Color.FromArgb(0, 66, 114);
            progressBar.Location = new Point(101, 222);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(541, 27);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 0;
            // 
            // progressLabel
            // 
            progressLabel.Anchor = AnchorStyles.Top;
            progressLabel.AutoSize = true;
            progressLabel.BackColor = Color.Black;
            progressLabel.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            progressLabel.ForeColor = Color.White;
            progressLabel.Location = new Point(101, 183);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new Size(131, 27);
            progressLabel.TabIndex = 1;
            progressLabel.Text = "scanning...";
            // 
            // encouragementLabel
            // 
            encouragementLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            encouragementLabel.BackColor = Color.Black;
            encouragementLabel.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point);
            encouragementLabel.ForeColor = Color.White;
            encouragementLabel.Location = new Point(0, 316);
            encouragementLabel.Name = "encouragementLabel";
            encouragementLabel.Size = new Size(765, 27);
            encouragementLabel.TabIndex = 2;
            encouragementLabel.Text = "nice!";
            encouragementLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ScanningScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(encouragementLabel);
            Controls.Add(progressLabel);
            Controls.Add(progressBar);
            Name = "ScanningScreen";
            Size = new Size(768, 648);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar;
        private Label progressLabel;
        private Label encouragementLabel;
    }
}
