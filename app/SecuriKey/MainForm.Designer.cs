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
            startButton = new Button();
            stopButton = new Button();
            statusTextbox = new TextBox();
            authorizeInternetConnectionCheckbox = new CheckBox();
            generateReportButton = new Button();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Location = new Point(50, 27);
            startButton.Name = "startButton";
            startButton.Size = new Size(117, 53);
            startButton.TabIndex = 0;
            startButton.Text = "Start";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += OnStartButtonClick;
            // 
            // stopButton
            // 
            stopButton.Location = new Point(221, 27);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(117, 53);
            stopButton.TabIndex = 1;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += OnStopButtonClick;
            // 
            // _statusTextbox
            // 
            statusTextbox.BackColor = Color.White;
            statusTextbox.Location = new Point(50, 149);
            statusTextbox.Multiline = true;
            statusTextbox.Name = "_statusTextbox";
            statusTextbox.ReadOnly = true;
            statusTextbox.ScrollBars = ScrollBars.Vertical;
            statusTextbox.Size = new Size(812, 423);
            statusTextbox.TabIndex = 2;
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
            // generateReportButton
            // 
            generateReportButton.Location = new Point(745, 27);
            generateReportButton.Name = "generateReportButton";
            generateReportButton.Size = new Size(117, 53);
            generateReportButton.TabIndex = 4;
            generateReportButton.Text = "Generate Report";
            generateReportButton.UseVisualStyleBackColor = true;
            generateReportButton.Click += OnGenerateReportButtonClick;
            // 
            // mainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(897, 584);
            Controls.Add(generateReportButton);
            Controls.Add(authorizeInternetConnectionCheckbox);
            Controls.Add(statusTextbox);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "mainForm";
            Text = "SecuriKey";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startButton;
        private Button stopButton;
        private TextBox statusTextbox;
        private CheckBox authorizeInternetConnectionCheckbox;
        private Button generateReportButton;
    }
}