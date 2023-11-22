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
            scanButton = new Controls.RoundedButton();
            aboutButton = new Controls.RoundedButton();
            backgroundPictureBox = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            allowInternetConnectionCheckbox = new CheckBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).BeginInit();
            SuspendLayout();
            // 
            // scanButton
            // 
            scanButton.Anchor = AnchorStyles.None;
            scanButton.BackColor = Color.Transparent;
            scanButton.BorderColor = Color.Green;
            scanButton.BorderRadius = 40;
            scanButton.BorderThickness = 5F;
            scanButton.ButtonText = "Scan Now";
            scanButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            scanButton.ForeColor = Color.White;
            scanButton.Location = new Point(445, 565);
            scanButton.Margin = new Padding(3, 4, 3, 4);
            scanButton.Name = "scanButton";
            scanButton.PressedColor = Color.FromArgb(0, 96, 166);
            scanButton.Size = new Size(300, 85);
            scanButton.TabIndex = 2;
            scanButton.UnpressedColor = Color.Black;
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
            aboutButton.Location = new Point(445, 658);
            aboutButton.Margin = new Padding(3, 4, 3, 4);
            aboutButton.Name = "aboutButton";
            aboutButton.PressedColor = Color.FromArgb(0, 96, 166);
            aboutButton.Size = new Size(300, 85);
            aboutButton.TabIndex = 4;
            aboutButton.UnpressedColor = Color.Black;
            aboutButton.Click += OnAboutButtonClick;
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
            // allowInternetConnectionCheckbox
            // 
            allowInternetConnectionCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            allowInternetConnectionCheckbox.BackColor = Color.Black;
            allowInternetConnectionCheckbox.Checked = true;
            allowInternetConnectionCheckbox.CheckState = CheckState.Checked;
            allowInternetConnectionCheckbox.ForeColor = Color.White;
            allowInternetConnectionCheckbox.Location = new Point(21, 806);
            allowInternetConnectionCheckbox.Name = "allowInternetConnectionCheckbox";
            allowInternetConnectionCheckbox.Size = new Size(252, 22);
            allowInternetConnectionCheckbox.TabIndex = 8;
            allowInternetConnectionCheckbox.Text = "Allow SecuriKey to use Internet connection";
            allowInternetConnectionCheckbox.UseVisualStyleBackColor = false;
            allowInternetConnectionCheckbox.CheckedChanged += OnAllowInternetConnectionCheckboxCheckedChanged;
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
            // 
            // HomeScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(label3);
            Controls.Add(allowInternetConnectionCheckbox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(aboutButton);
            Controls.Add(scanButton);
            Controls.Add(backgroundPictureBox);
            ForeColor = SystemColors.ControlText;
            Name = "HomeScreen";
            Size = new Size(1206, 838);
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Controls.RoundedButton offlineScanButton;
        private Controls.RoundedButton scanButton;
        private Controls.RoundedButton aboutButton;
        private PictureBox logoPictureBox;
        private PictureBox backgroundPictureBox;
        private Label label1;
        private Label label2;
        private CheckBox allowInternetConnectionCheckbox;
        private Label label3;
    }
}
