namespace SecuriKey
{
    partial class mainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _startButton = new Button();
            _stopButton = new Button();
            _statusTextbox = new TextBox();
            authorizeInternetConnectionCheckbox = new CheckBox();
            SuspendLayout();
            // 
            // _startButton
            // 
            _startButton.Location = new Point(50, 27);
            _startButton.Name = "_startButton";
            _startButton.Size = new Size(117, 53);
            _startButton.TabIndex = 0;
            _startButton.Text = "Start";
            _startButton.UseVisualStyleBackColor = true;
            _startButton.Click += OnStartButtonClick;
            // 
            // _stopButton
            // 
            _stopButton.Location = new Point(221, 27);
            _stopButton.Name = "_stopButton";
            _stopButton.Size = new Size(117, 53);
            _stopButton.TabIndex = 1;
            _stopButton.Text = "Stop";
            _stopButton.UseVisualStyleBackColor = true;
            _stopButton.Click += OnStopButtonClick;
            // 
            // _statusTextbox
            // 
            _statusTextbox.BackColor = Color.White;
            _statusTextbox.Location = new Point(50, 149);
            _statusTextbox.Multiline = true;
            _statusTextbox.Name = "_statusTextbox";
            _statusTextbox.ReadOnly = true;
            _statusTextbox.ScrollBars = ScrollBars.Vertical;
            _statusTextbox.Size = new Size(812, 423);
            _statusTextbox.TabIndex = 2;
            // 
            // authorizeInternetConnectionCheckbox
            // 
            authorizeInternetConnectionCheckbox.AutoSize = true;
            authorizeInternetConnectionCheckbox.Location = new Point(50, 105);
            authorizeInternetConnectionCheckbox.Name = "authorizeInternetConnectionCheckbox";
            authorizeInternetConnectionCheckbox.Size = new Size(186, 19);
            authorizeInternetConnectionCheckbox.TabIndex = 3;
            authorizeInternetConnectionCheckbox.Text = "Authorize Internet Connection";
            authorizeInternetConnectionCheckbox.UseVisualStyleBackColor = true;
            authorizeInternetConnectionCheckbox.CheckedChanged += OnAuthorizeInternetConnectionCheckboxCheckedChanged;
            // 
            // mainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(897, 584);
            Controls.Add(authorizeInternetConnectionCheckbox);
            Controls.Add(_statusTextbox);
            Controls.Add(_stopButton);
            Controls.Add(_startButton);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "mainForm";
            Text = "SecuriKey";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button _startButton;
        private Button _stopButton;
        private TextBox _statusTextbox;
        private CheckBox authorizeInternetConnectionCheckbox;
    }
}