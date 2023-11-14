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
            label1 = new Label();
            pleaseWait = new Label();
            aiResponse = new RichTextBox();
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(522, 22);
            label1.Name = "label1";
            label1.Size = new Size(457, 21);
            label1.TabIndex = 1;
            label1.Text = "Hello, I am your AI assistant. I'll help you mitigate this risk.";
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
            // aiResponse
            // 
            aiResponse.BackColor = Color.FromArgb(64, 64, 64);
            aiResponse.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            aiResponse.ForeColor = Color.White;
            aiResponse.Location = new Point(531, 112);
            aiResponse.Name = "aiResponse";
            aiResponse.Size = new Size(459, 722);
            aiResponse.TabIndex = 3;
            aiResponse.Text = "";
            aiResponse.Visible = false;
            aiResponse.TextChanged += aiResponse_TextChanged;
            // 
            // AiAssistant
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1008, 857);
            Controls.Add(aiResponse);
            Controls.Add(pleaseWait);
            Controls.Add(label1);
            Controls.Add(aiAssistantImage);
            ForeColor = Color.Black;
            MaximumSize = new Size(1024, 896);
            MinimumSize = new Size(1024, 894);
            Name = "AiAssistant";
            ((System.ComponentModel.ISupportInitialize)aiAssistantImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox aiAssistantImage;
        private Label label1;
        private Label label2;
        private RichTextBox aiResponse;
        private Label pleaseWait;
    }
}