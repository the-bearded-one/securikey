namespace SecuriKey.Screens
{
    partial class AiAssistant
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
            aiAssistantImage = new PictureBox();
            introLabel = new Label();
            pleaseWait = new Label();
            chatWindow = new RichTextBox();
            inputTextBox = new TextBox();
            sendButton = new Button();
            ((System.ComponentModel.ISupportInitialize)aiAssistantImage).BeginInit();
            SuspendLayout();
            // 
            // aiAssistantImage
            // 
            aiAssistantImage.Anchor = AnchorStyles.Left;
            aiAssistantImage.Image = Resources.Resources.assistant01;
            aiAssistantImage.Location = new Point(0, 4);
            aiAssistantImage.Name = "aiAssistantImage";
            aiAssistantImage.Size = new Size(516, 856);
            aiAssistantImage.TabIndex = 0;
            aiAssistantImage.TabStop = false;
            // 
            // introLabel
            // 
            introLabel.AutoSize = true;
            introLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            introLabel.ForeColor = Color.White;
            introLabel.Location = new Point(522, 22);
            introLabel.Name = "introLabel";
            introLabel.Size = new Size(457, 21);
            introLabel.TabIndex = 1;
            introLabel.Text = "Hello, I am your AI assistant. I'll help you mitigate this risk.";
            // 
            // pleaseWait
            // 
            pleaseWait.AutoSize = true;
            pleaseWait.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            pleaseWait.ForeColor = Color.White;
            pleaseWait.Location = new Point(523, 57);
            pleaseWait.Name = "pleaseWait";
            pleaseWait.Size = new Size(250, 21);
            pleaseWait.TabIndex = 2;
            pleaseWait.Text = "Please give me just a moment...";
            // 
            // chatWindow
            // 
            chatWindow.BackColor = Color.FromArgb(64, 64, 64);
            chatWindow.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            chatWindow.ForeColor = Color.White;
            chatWindow.Location = new Point(531, 112);
            chatWindow.Name = "chatWindow";
            chatWindow.ReadOnly = true;
            chatWindow.Size = new Size(459, 647);
            chatWindow.TabIndex = 3;
            chatWindow.Text = "";
            // 
            // inputTextBox
            // 
            inputTextBox.Location = new Point(531, 776);
            inputTextBox.Multiline = true;
            inputTextBox.Name = "inputTextBox";
            inputTextBox.PlaceholderText = "<Ask your questions here>";
            inputTextBox.Size = new Size(389, 69);
            inputTextBox.TabIndex = 4;
            inputTextBox.KeyDown += OnInputTextBoxKeyDown;
            // 
            // sendButton
            // 
            sendButton.BackColor = Color.FromArgb(128, 128, 255);
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            sendButton.Location = new Point(926, 776);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(64, 69);
            sendButton.TabIndex = 5;
            sendButton.Text = " ▶";
            sendButton.UseVisualStyleBackColor = false;
            sendButton.Click += OnSendButtonClick;
            // 
            // AiAssistant
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1008, 857);
            Controls.Add(sendButton);
            Controls.Add(inputTextBox);
            Controls.Add(chatWindow);
            Controls.Add(pleaseWait);
            Controls.Add(introLabel);
            Controls.Add(aiAssistantImage);
            ForeColor = Color.Black;
            MaximumSize = new Size(1024, 896);
            MinimumSize = new Size(1024, 894);
            Name = "AiAssistant";
            Shown += OnAiAssistantShown;
            ((System.ComponentModel.ISupportInitialize)aiAssistantImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox aiAssistantImage;
        private Label introLabel;
        private Label label2;
        private RichTextBox chatWindow;
        private Label pleaseWait;
        private TextBox inputTextBox;
        private Button sendButton;
    }
}