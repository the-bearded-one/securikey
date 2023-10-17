using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecuriKey.Controls
{
    public partial class RoundedButton : UserControl
    {
        bool isPressed = false;

        public RoundedButton()
        {
            InitializeComponent();

            // we have textLabel docked on top of us, so we need to bubble up all its mouse events
            textLabel.MouseClick += OnRoundedButtonMouseClick;
            textLabel.MouseDown += OnRoundedButtonMouseDown;
            textLabel.MouseUp += OnRoundedButtonMouseUp;
        }

        public string ButtonText
        {
            get
            {
                return textLabel.Text;
            }
            set
            {
                textLabel.Text = value;
            }
        }

        public int BorderRadius { get; set; } = 40;
        public float BorderThickness { get; set; } = 5;
        public Color PressedColor { get; set; } = Color.FromArgb(0, 96, 166);
        public Color BorderColor { get; set; } = Color.FromArgb(0, 66, 114);
        public Color UnpressedColor { get; set; } = Color.Black;

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var unpressedBrush = new SolidBrush(this.UnpressedColor))
            using (var pressedBrush = new SolidBrush(this.PressedColor))
            using (var borderPen = new Pen(BorderColor, BorderThickness))
            using (var graphicsPath = new GraphicsPath())
            {
                // Create a rounded rectangle path
                int BorderWidthInt = Convert.ToInt32(BorderThickness);
                var rect = new Rectangle(BorderWidthInt / 2, BorderWidthInt / 2, Width - BorderWidthInt, Height - BorderWidthInt);
                graphicsPath.AddArc(rect.Left, rect.Top, BorderRadius * 2, BorderRadius * 2, 180, 90); // Top left corner
                graphicsPath.AddArc(rect.Right - BorderRadius * 2, rect.Top, BorderRadius * 2, BorderRadius * 2, 270, 90); // Top right corner
                graphicsPath.AddArc(rect.Right - BorderRadius * 2, rect.Bottom - BorderRadius * 2, BorderRadius * 2, BorderRadius * 2, 0, 90); // Bottom right corner
                graphicsPath.AddArc(rect.Left, rect.Bottom - BorderRadius * 2, BorderRadius * 2, BorderRadius * 2, 90, 90); // Bottom left corner
                graphicsPath.CloseAllFigures();

                // Fill the button with a flat color
                if (isPressed) e.Graphics.FillPath(pressedBrush, graphicsPath);
                else e.Graphics.FillPath(unpressedBrush, graphicsPath);

                // Draw the border
                e.Graphics.DrawPath(borderPen, graphicsPath);
            }

            base.OnPaint(e);
        }

        private void OnRoundedButtonMouseClick(object sender, MouseEventArgs e)
        {
            isPressed = false;
            this.OnClick(e); // bubble event up to parent
            this.Invalidate();
        }

        private void OnRoundedButtonMouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            this.Invalidate();
        }

        private void OnRoundedButtonMouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
            this.Invalidate();
        }
    }
}