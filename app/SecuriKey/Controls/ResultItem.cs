using BusinessLogic;
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
    public partial class ResultItem : UserControl
    {
        int squashedHeight = 0;

        public ResultItem()
        {
            InitializeComponent();

            // remember our starting height
            squashedHeight = this.Height;

            // bubble up click events to this control
            scanTypeLabel.Click += OnResultItemClick;
            shortDescriptionLabel.Click += OnResultItemClick;
            severityIndicator.Click += OnResultItemClick;
        }

        bool IsSquashed { get; set; } = false;
        public Severity Severity
        {
            get => severityIndicator.Severity;
            set => severityIndicator.Severity = value;
        }

        private void OnResultItemClick(object sender, EventArgs e)
        {
            IsSquashed = !IsSquashed;

            if (IsSquashed) this.Height = squashedHeight;
            else this.Height = detailsTextbox.Bottom + 12;
        }
    }
}
