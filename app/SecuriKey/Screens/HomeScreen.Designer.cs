﻿namespace SecuriKey.Screens
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
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).BeginInit();
            SuspendLayout();
            // 
            // offlineScanButton
            // 
            offlineScanButton.Anchor = AnchorStyles.None;
            offlineScanButton.BackColor = Color.Transparent;
            offlineScanButton.BorderColor = Color.FromArgb(0, 66, 114);
            offlineScanButton.BorderRadius = 40;
            offlineScanButton.BorderThickness = 5F;
            offlineScanButton.ButtonText = "Scan Offline";
            offlineScanButton.ForeColor = Color.White;
            offlineScanButton.Location = new Point(226, 347);
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
            onlineScanButton.Location = new Point(226, 450);
            onlineScanButton.Margin = new Padding(3, 4, 3, 4);
            onlineScanButton.Name = "onlineScanButton";
            onlineScanButton.PressedColor = Color.FromArgb(0, 96, 166);
            onlineScanButton.Size = new Size(300, 85);
            onlineScanButton.TabIndex = 3;
            onlineScanButton.UnpressedColor = Color.Black;
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
            aboutButton.Location = new Point(226, 550);
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
            logoPictureBox.Location = new Point(161, -84);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(452, 461);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.TabIndex = 1;
            logoPictureBox.TabStop = false;
            // 
            // backgroundPictureBox
            // 
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.Image = Resources.Resources.bg;
            backgroundPictureBox.Location = new Point(0, 0);
            backgroundPictureBox.Name = "backgroundPictureBox";
            backgroundPictureBox.Size = new Size(800, 600);
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            backgroundPictureBox.TabIndex = 5;
            backgroundPictureBox.TabStop = false;
            // 
            // HomeScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(aboutButton);
            Controls.Add(onlineScanButton);
            Controls.Add(offlineScanButton);
            Controls.Add(logoPictureBox);
            Controls.Add(backgroundPictureBox);
            ForeColor = SystemColors.ControlText;
            Name = "HomeScreen";
            Size = new Size(800, 600);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Controls.RoundedButton offlineScanButton;
        private Controls.RoundedButton onlineScanButton;
        private Controls.RoundedButton aboutButton;
        private PictureBox logoPictureBox;
        private PictureBox backgroundPictureBox;
    }
}
