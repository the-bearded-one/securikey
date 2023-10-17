using BusinessLogic;
using PdfSharp.Drawing;
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
    public partial class SeverityIndicator : UserControl
    {
        public SeverityIndicator()
        {
            InitializeComponent();
        }

        public int BorderRadius { get; set; } = 15;
        public Severity Severity { get; set; } = Severity.Low;
        public Color OkColor { get; set; } = Color.FromArgb(61, 131, 97);
        public Color HighColor { get; set; } = Color.FromArgb(168, 37, 33);
        public Color MediumColor { get; set; } = Color.FromArgb(209, 130, 36);
        public Color LowColor { get; set; } = Color.FromArgb(135, 133, 156);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // configure look based on severity
            Color fillColor = OkColor;
            if (this.Severity == Severity.Low) fillColor = LowColor;
            else if (this.Severity == Severity.Medium) fillColor = MediumColor;
            else if (this.Severity == Severity.High) fillColor = HighColor;

            var fontSize = GetMaxFontSize(this.Severity.ToString(), this.Font);
            
            using (var fillBrush = new SolidBrush(fillColor))
            using (var graphicsPath = new GraphicsPath())
            using (var font = new Font(this.Font.FontFamily, fontSize, FontStyle.Regular))
            using (var textBrush = new SolidBrush(this.ForeColor))
            {
                // Create a rounded rectangle path
                var rect = new Rectangle(0, 0, Width, Height);
                graphicsPath.AddArc(rect.Left, rect.Top, BorderRadius * 2, BorderRadius * 2, 180, 90); // Top left corner
                graphicsPath.AddArc(rect.Right - BorderRadius * 2, rect.Top, BorderRadius * 2, BorderRadius * 2, 270, 90); // Top right corner
                graphicsPath.AddArc(rect.Right - BorderRadius * 2, rect.Bottom - BorderRadius * 2, BorderRadius * 2, BorderRadius * 2, 0, 90); // Bottom right corner
                graphicsPath.AddArc(rect.Left, rect.Bottom - BorderRadius * 2, BorderRadius * 2, BorderRadius * 2, 90, 90); // Bottom left corner
                graphicsPath.CloseAllFigures();

                // Fill the pill with color based on severity
                e.Graphics.FillPath(fillBrush, graphicsPath);

                // Create a StringFormat to set text alignment
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,    // Right-justified
                    LineAlignment = StringAlignment.Center  // Vertically centered
                };

                // Calculate the position for the text
                RectangleF textRect = new RectangleF(0, 0, Width, Height); // Use the control's dimensions
                e.Graphics.DrawString(this.Severity.ToString(), font, textBrush, textRect, format);
            }
        }

        /// <summary>
        /// Get max font size to fit the height of the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public float GetMaxFontSize(string text, Font font)
        {
            // Create a Graphics object for measuring text
            using (var graphics = this.CreateGraphics())
            {
                for (float fontSize = 1; fontSize <= 100; fontSize += 0.5f) // You can adjust the range and step
                {
                    var testFont = new Font(font.FontFamily, fontSize, font.Style);

                    // Measure the size of the text with the test font
                    SizeF textSize = graphics.MeasureString(text, testFont);

                    // Check if the text fits within the this
                    if (textSize.Height > this.Height)
                    {
                        testFont.Dispose(); // Release the test font resource
                        return fontSize - 0.5f; // The previous font size fits
                    }

                    testFont.Dispose(); // Release the test font resource
                }
            }

            // If no suitable font size is found, return the maximum size
            return 100; // You can adjust this value based on your needs
        }
    }
}
