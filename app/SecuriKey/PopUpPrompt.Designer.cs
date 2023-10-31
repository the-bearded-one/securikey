namespace SecuriKey
{
    partial class PopUpPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopUpPrompt));
            cancelButton = new Controls.RoundedButton();
            okButton = new Controls.RoundedButton();
            detailsTextBox = new RichTextBox();
            inputTextBox = new TextBox();
            errorLabel = new Label();
            SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.Transparent;
            cancelButton.BorderColor = Color.FromArgb(0, 66, 114);
            cancelButton.BorderRadius = 20;
            cancelButton.BorderThickness = 5F;
            cancelButton.ButtonText = "Cancel";
            cancelButton.ForeColor = Color.White;
            cancelButton.Location = new Point(52, 349);
            cancelButton.Margin = new Padding(3, 4, 3, 4);
            cancelButton.Name = "cancelButton";
            cancelButton.PressedColor = Color.FromArgb(0, 96, 166);
            cancelButton.Size = new Size(172, 64);
            cancelButton.TabIndex = 0;
            cancelButton.UnpressedColor = Color.FromArgb(89, 152, 255);
            cancelButton.Click += OnCancelButtonClick;
            // 
            // okButton
            // 
            okButton.BackColor = Color.Transparent;
            okButton.BorderColor = Color.FromArgb(0, 66, 114);
            okButton.BorderRadius = 20;
            okButton.BorderThickness = 5F;
            okButton.ButtonText = "OK";
            okButton.ForeColor = Color.White;
            okButton.Location = new Point(406, 349);
            okButton.Margin = new Padding(3, 4, 3, 4);
            okButton.Name = "okButton";
            okButton.PressedColor = Color.FromArgb(0, 96, 166);
            okButton.Size = new Size(172, 64);
            okButton.TabIndex = 1;
            okButton.UnpressedColor = Color.FromArgb(89, 152, 255);
            okButton.Click += OnOkButtonClick;
            // 
            // detailsTextBox
            // 
            detailsTextBox.BackColor = Color.Black;
            detailsTextBox.BorderStyle = BorderStyle.None;
            detailsTextBox.ForeColor = Color.White;
            detailsTextBox.Location = new Point(39, 25);
            detailsTextBox.Name = "detailsTextBox";
            detailsTextBox.ReadOnly = true;
            detailsTextBox.Size = new Size(557, 169);
            detailsTextBox.TabIndex = 3;
            detailsTextBox.Text = resources.GetString("detailsTextBox.Text");
            // 
            // inputTextBox
            // 
            inputTextBox.Location = new Point(125, 229);
            inputTextBox.Name = "inputTextBox";
            inputTextBox.PlaceholderText = "<input here>";
            inputTextBox.Size = new Size(364, 27);
            inputTextBox.TabIndex = 2;
            inputTextBox.Text = " ";
            inputTextBox.KeyUp += OnInputKeyUp;
            // 
            // errorLabel
            // 
            errorLabel.ForeColor = Color.Red;
            errorLabel.Location = new Point(12, 283);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(600, 23);
            errorLabel.TabIndex = 4;
            errorLabel.Text = "Invalid password. Please check that password meet minimum requirements!";
            errorLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PopUpPrompt
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            ClientSize = new Size(624, 441);
            ControlBox = false;
            Controls.Add(errorLabel);
            Controls.Add(detailsTextBox);
            Controls.Add(inputTextBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "PopUpPrompt";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Show;
            Text = "PopUpPrompt";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.RoundedButton cancelButton;
        private Controls.RoundedButton okButton;
        private RichTextBox detailsTextBox;
        private TextBox inputTextBox;
        private Label errorLabel;
    }
}