namespace SecuriKey.Controls
{
    partial class RoundedButton
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
            textLabel = new Label();
            SuspendLayout();
            // 
            // textLabel
            // 
            textLabel.BackColor = Color.Transparent;
            textLabel.Dock = DockStyle.Fill;
            textLabel.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            textLabel.Location = new Point(0, 0);
            textLabel.Name = "textLabel";
            textLabel.Size = new Size(322, 92);
            textLabel.TabIndex = 0;
            textLabel.Text = "textLabel";
            textLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RoundedButton
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(textLabel);
            ForeColor = Color.White;
            Name = "RoundedButton";
            Size = new Size(322, 92);
            MouseClick += OnRoundedButtonMouseClick;
            MouseDown += OnRoundedButtonMouseDown;
            MouseUp += OnRoundedButtonMouseUp;
            ResumeLayout(false);
        }

        #endregion

        private Label textLabel;
    }
}
