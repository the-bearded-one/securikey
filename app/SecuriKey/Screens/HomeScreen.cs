using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecuriKey.Screens
{
    public partial class HomeScreen : UserControl, IScreen
    {
        public HomeScreen()
        {
            InitializeComponent();

            SuspendLayout();

            // to get transparency, the parent of controls need to be the background. so set the background image of this screen to parent of other screens
            // // put all controls into a seperate list since we will be iterating over them
            List<Control> ctrls = new List<Control>();
            foreach (Control ctrl in Controls)
            {
                ctrls.Add(ctrl);
            }
            // // now iterate over the controls and change the parent
            foreach (var ctrl in ctrls)
            {
                if (ctrl != this.backgroundPictureBox) ctrl.Parent = this.backgroundPictureBox;
            }

            ResumeLayout();

            offlineScanButton.Click += OnOfflineScanButtonClick;
            onlineScanButton.Click += OnOnlineScanButtonClick;
        }

        public event EventHandler<NavigationEventArgs> NavigationRequest;

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

        private void OnAboutButtonClick(object sender, EventArgs e)
        {
            NavigationRequest?.Invoke(this, new NavigationEventArgs(new  AboutScreen()));
        }

        public UserControl AsUserControl()
        {
            return this;
        }
    }
}
