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
            offlineScanButton = new Controls.RoundedButton();
            onlineScanButton = new Controls.RoundedButton();
            aboutButton = new Controls.RoundedButton();
            logoPictureBox = new PictureBox();
            backgroundPictureBox = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            checkBox1 = new CheckBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).BeginInit();
            SuspendLayout();
            // 
            // offlineScanButton
            // 
            offlineScanButton.Anchor = AnchorStyles.None;
            offlineScanButton.BackColor = Color.Transparent;
            offlineScanButton.BorderColor = Color.Green;
            offlineScanButton.BorderRadius = 40;
            offlineScanButton.BorderThickness = 5F;
            offlineScanButton.ButtonText = "Scan Now";
            offlineScanButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            offlineScanButton.ForeColor = Color.White;
            offlineScanButton.Location = new Point(445, 581);
            offlineScanButton.Margin = new Padding(3, 4, 3, 4);
            offlineScanButton.Name = "offlineScanButton";
            offlineScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            offlineScanButton.Size = new Size(300, 85);
            offlineScanButton.TabIndex = 2;
            offlineScanButton.UnpressedColor = Color.Black;
            // 
            // onlineScanButton
            // 
            onlineScanButton.Anchor = AnchorStyles.None;
            onlineScanButton.BackColor = Color.Transparent;
            onlineScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            onlineScanButton.BorderRadius = 40;
            onlineScanButton.BorderThickness = 5F;
            onlineScanButton.ButtonText = "Scan Online";
            onlineScanButton.ForeColor = Color.White;
            onlineScanButton.Location = new Point(21, 694);
            onlineScanButton.Margin = new Padding(3, 4, 3, 4);
            onlineScanButton.Name = "onlineScanButton";
            onlineScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            onlineScanButton.Size = new Size(300, 85);
            onlineScanButton.TabIndex = 3;
            onlineScanButton.UnpressedColor = Color.Black;
            onlineScanButton.Visible = false;
            // 
            // aboutButton
            // 
            aboutButton.Anchor = AnchorStyles.None;
            aboutButton.BackColor = Color.Transparent;
            aboutButton.BorderColor = Color.FromArgb(0, 66, 114);
            aboutButton.BorderRadius = 40;
            aboutButton.BorderThickness = 5F;
            aboutButton.ButtonText = "About";
            aboutButton.ForeColor = Color.White;
            aboutButton.Location = new Point(445, 673);
            aboutButton.Margin = new Padding(3, 4, 3, 4);
            aboutButton.Name = "aboutButton";
            aboutButton.PressedColor = Color.FromArgb(0, 96, 166);
            aboutButton.Size = new Size(300, 85);
            aboutButton.TabIndex = 4;
            aboutButton.UnpressedColor = Color.Black;
            aboutButton.Click += OnAboutButtonClick;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Anchor = AnchorStyles.None;
            logoPictureBox.BackColor = Color.Transparent;
            logoPictureBox.BackgroundImageLayout = ImageLayout.None;
            logoPictureBox.Image = Resources.Resources.SecuriKeyLogo;
            logoPictureBox.Location = new Point(364, 35);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(452, 461);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.TabIndex = 1;
            logoPictureBox.TabStop = false;
            logoPictureBox.Visible = false;
            // 
            // backgroundPictureBox
            // 
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.Image = Resources.Resources.bg2;
            backgroundPictureBox.Location = new Point(0, 0);
            backgroundPictureBox.Name = "backgroundPictureBox";
            backgroundPictureBox.Size = new Size(1206, 838);
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            backgroundPictureBox.TabIndex = 5;
            backgroundPictureBox.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Black;
            label1.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.MenuHighlight;
            label1.Location = new Point(8, 7);
            label1.Name = "label1";
            label1.Size = new Size(155, 41);
            label1.TabIndex = 6;
            label1.Text = "SecuriKey";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.BackColor = Color.Black;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(1113, 12);
            label2.Name = "label2";
            label2.Size = new Size(109, 25);
            label2.TabIndex = 7;
            label2.Text = "v1.0 BETA  ";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox1.BackColor = Color.Black;
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(21, 806);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(252, 19);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "Allow SecuriKey to use Internet connection";
            checkBox1.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.BackColor = Color.Black;
            label3.Font = new Font("Segoe UI", 17F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.White;
            label3.Location = new Point(756, 797);
            label3.Name = "label3";
            label3.Size = new Size(450, 31);
            label3.TabIndex = 9;
            label3.Text = "Assess your PC against 39 attack vectors!";
            label3.Click += label3_Click;
            // 
            // HomeScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(label3);
            Controls.Add(checkBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(aboutButton);
            Controls.Add(onlineScanButton);
            Controls.Add(offlineScanButton);
            Controls.Add(logoPictureBox);
            Controls.Add(backgroundPictureBox);
            ForeColor = SystemColors.ControlText;
            Name = "HomeScreen";
            Size = new Size(1206, 838);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Controls.RoundedButton offlineScanButton;
        private Controls.RoundedButton onlineScanButton;
        private Controls.RoundedButton aboutButton;
        private PictureBox logoPictureBox;
        private PictureBox backgroundPictureBox;
        private Label label1;
        private Label label2;
        private CheckBox checkBox1;
        private Label label3;
    }
}
