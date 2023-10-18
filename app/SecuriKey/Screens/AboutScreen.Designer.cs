namespace SecuriKey.Screens
{
    partial class AboutScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutScreen));
            urlLabel = new Label();
            backgroundPictureBox = new PictureBox();
            githubLabel = new Label();
            infoLabel = new Controls.MultiLineLabel();
            subtitleLabel = new Label();
            titlePictureBox = new PictureBox();
            backScanButton = new Controls.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)titlePictureBox).BeginInit();
            SuspendLayout();
            // 
            // urlLabel
            // 
            urlLabel.Anchor = AnchorStyles.None;
            urlLabel.BackColor = Color.Transparent;
            urlLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            urlLabel.ForeColor = Color.White;
            urlLabel.Location = new Point(28, 458);
            urlLabel.Name = "urlLabel";
            urlLabel.Size = new Size(753, 28);
            urlLabel.TabIndex = 2;
            urlLabel.Text = "tinyurl.com/securikey-mics";
            urlLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // backgroundPictureBox
            // 
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.Image = Resources.Resources.bg;
            backgroundPictureBox.Location = new Point(0, 0);
            backgroundPictureBox.Name = "backgroundPictureBox";
            backgroundPictureBox.Size = new Size(800, 600);
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            backgroundPictureBox.TabIndex = 3;
            backgroundPictureBox.TabStop = false;
            // 
            // githubLabel
            // 
            githubLabel.Anchor = AnchorStyles.None;
            githubLabel.BackColor = Color.Transparent;
            githubLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            githubLabel.ForeColor = Color.White;
            githubLabel.Location = new Point(28, 482);
            githubLabel.Name = "githubLabel";
            githubLabel.Size = new Size(753, 28);
            githubLabel.TabIndex = 4;
            githubLabel.Text = "https://github.com/the-bearded-one/securikey";
            githubLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // infoLabel
            // 
            infoLabel.Anchor = AnchorStyles.None;
            infoLabel.BackColor = Color.Transparent;
            infoLabel.DisplayText = resources.GetString("infoLabel.DisplayText");
            infoLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            infoLabel.ForeColor = Color.White;
            infoLabel.Location = new Point(28, 221);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(753, 117);
            infoLabel.StringAlignment = StringAlignment.Center;
            infoLabel.TabIndex = 5;
            // 
            // subtitleLabel
            // 
            subtitleLabel.Anchor = AnchorStyles.None;
            subtitleLabel.BackColor = Color.Transparent;
            subtitleLabel.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            subtitleLabel.ForeColor = Color.White;
            subtitleLabel.Location = new Point(28, 155);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(753, 36);
            subtitleLabel.TabIndex = 6;
            subtitleLabel.Text = "Lightweight Security at Your Fingertips";
            subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // titlePictureBox
            // 
            titlePictureBox.Anchor = AnchorStyles.None;
            titlePictureBox.BackColor = Color.Transparent;
            titlePictureBox.Image = Resources.Resources.AboutSecuriKeyTitle;
            titlePictureBox.Location = new Point(179, 76);
            titlePictureBox.Name = "titlePictureBox";
            titlePictureBox.Size = new Size(446, 65);
            titlePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            titlePictureBox.TabIndex = 7;
            titlePictureBox.TabStop = false;
            // 
            // backScanButton
            // 
            backScanButton.Anchor = AnchorStyles.None;
            backScanButton.BackColor = Color.Transparent;
            backScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            backScanButton.BorderRadius = 20;
            backScanButton.BorderThickness = 5F;
            backScanButton.ButtonText = "Back";
            backScanButton.ForeColor = Color.White;
            backScanButton.Location = new Point(288, 530);
            backScanButton.Margin = new Padding(3, 5, 3, 5);
            backScanButton.Name = "backScanButton";
            backScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            backScanButton.Size = new Size(212, 70);
            backScanButton.TabIndex = 8;
            backScanButton.UnpressedColor = Color.Black;
            backScanButton.Click += OnBackScanButtonClick;
            // 
            // AboutScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(backScanButton);
            Controls.Add(titlePictureBox);
            Controls.Add(subtitleLabel);
            Controls.Add(infoLabel);
            Controls.Add(githubLabel);
            Controls.Add(urlLabel);
            Controls.Add(backgroundPictureBox);
            Margin = new Padding(3, 4, 3, 4);
            Name = "AboutScreen";
            Size = new Size(800, 600);
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)titlePictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar progressBar;
        private Label progressLabel;
        private Label urlLabel;
        private PictureBox backgroundPictureBox;
        private Label githubLabel;
        private Controls.MultiLineLabel infoLabel;
        private Label subtitleLabel;
        private PictureBox titlePictureBox;
        private Controls.RoundedButton backScanButton;
    }
}
