namespace SecuriKey.Screens
{
    partial class HomeScreen
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
            logoPictureBox = new PictureBox();
            offlineScanButton = new Controls.RoundedButton();
            onlineScanButton = new Controls.RoundedButton();
            aboutButton = new Controls.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // logoPictureBox
            // 
            logoPictureBox.Anchor = AnchorStyles.None;
            logoPictureBox.BackgroundImageLayout = ImageLayout.None;
            logoPictureBox.Image = Resources.Resources.SecuriKeyLogo;
            logoPictureBox.Location = new Point(215, -60);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(374, 362);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.TabIndex = 1;
            logoPictureBox.TabStop = false;
            // 
            // offlineScanButton
            // 
            offlineScanButton.Anchor = AnchorStyles.None;
            offlineScanButton.BackColor = Color.Black;
            offlineScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            offlineScanButton.BorderRadius = 40;
            offlineScanButton.BorderThickness = 5F;
            offlineScanButton.ButtonText = "Scan Offline";
            offlineScanButton.ForeColor = Color.White;
            offlineScanButton.Location = new Point(242, 283);
            offlineScanButton.Margin = new Padding(3, 4, 3, 4);
            offlineScanButton.Name = "offlineScanButton";
            offlineScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            offlineScanButton.Size = new Size(300, 85);
            offlineScanButton.TabIndex = 2;
            // 
            // onlineScanButton
            // 
            onlineScanButton.Anchor = AnchorStyles.None;
            onlineScanButton.BackColor = Color.Black;
            onlineScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            onlineScanButton.BorderRadius = 40;
            onlineScanButton.BorderThickness = 5F;
            onlineScanButton.ButtonText = "Scan Online";
            onlineScanButton.ForeColor = Color.White;
            onlineScanButton.Location = new Point(242, 386);
            onlineScanButton.Margin = new Padding(3, 4, 3, 4);
            onlineScanButton.Name = "onlineScanButton";
            onlineScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            onlineScanButton.Size = new Size(300, 85);
            onlineScanButton.TabIndex = 3;
            // 
            // aboutButton
            // 
            aboutButton.Anchor = AnchorStyles.None;
            aboutButton.BackColor = Color.Black;
            aboutButton.BorderColor = Color.FromArgb(0, 66, 114);
            aboutButton.BorderRadius = 40;
            aboutButton.BorderThickness = 5F;
            aboutButton.ButtonText = "About";
            aboutButton.ForeColor = Color.White;
            aboutButton.Location = new Point(242, 486);
            aboutButton.Margin = new Padding(3, 4, 3, 4);
            aboutButton.Name = "aboutButton";
            aboutButton.PressedColor = Color.FromArgb(0, 96, 166);
            aboutButton.Size = new Size(300, 85);
            aboutButton.TabIndex = 4;
            // 
            // HomeScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(aboutButton);
            Controls.Add(onlineScanButton);
            Controls.Add(offlineScanButton);
            Controls.Add(logoPictureBox);
            ForeColor = SystemColors.ControlText;
            Name = "HomeScreen";
            Size = new Size(800, 600);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox logoPictureBox;
        private Controls.RoundedButton offlineScanButton;
        private Controls.RoundedButton onlineScanButton;
        private Controls.RoundedButton aboutButton;
    }
}
