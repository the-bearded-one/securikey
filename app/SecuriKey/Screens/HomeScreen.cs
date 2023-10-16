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

namespace SecuriKey.Screens
{
    public partial class HomeScreen : UserControl
    {
        public HomeScreen()
        {
            InitializeComponent();

            offlineScanButton.Click += OnOfflineScanButtonClick;

            onlineScanButton.Click += OnOnlineScanButtonClick;
        }

        private void OnOfflineScanButtonClick(object? sender, EventArgs e)
        {
            BL.Instance.IsInternetConnectionAuthorized = false;
            BL.Instance.StartSystemScan();
        }

        private void OnOnlineScanButtonClick(object? sender, EventArgs e)
        {
            BL.Instance.IsInternetConnectionAuthorized = true;
            BL.Instance.StartSystemScan();
        }
    }
}
