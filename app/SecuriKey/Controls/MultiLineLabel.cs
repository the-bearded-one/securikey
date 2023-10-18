using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecuriKey.Controls
{
    public partial class MultiLineLabel : UserControl
    {
        public MultiLineLabel()
        {
            InitializeComponent();
        }

        public string DisplayText
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        public StringAlignment StringAlignment { get; set; } = StringAlignment.Near;

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(this.ForeColor))
            {
                StringFormat format = new StringFormat();
                format.Alignment = this.StringAlignment;
                e.Graphics.DrawString(this.DisplayText, this.Font, brush, ClientRectangle, format);
            }
            base.OnPaint(e);
        }
    }
}
