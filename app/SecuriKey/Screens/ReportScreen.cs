using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using BusinessLogic;

namespace SecuriKey.Screens
{
    public partial class ReportScreen : UserControl, IScreen
    {

        public ReportScreen()
        {
            InitializeComponent();

            reportButton.Click += OnReportButtonClick;
            newScanButton.Click += OnNewScanButtonClick; ;

            foreach (ScanResult result in BL.Instance.ScanResults)
            {
                var resultCtl = new Controls.ResultItem(result);
                resultCtl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                resultsPanel.Controls.Add(resultCtl);
            }
        }

        public event EventHandler<NavigationEventArgs> NavigationRequest;

        public string ReportText
        {
            get
            {
                return string.Empty;
            }
            set
            {
                var str = value;
            }
        }

        private void OnNewScanButtonClick(object? sender, EventArgs e)
        {
            NavigationRequest?.Invoke(this, new NavigationEventArgs(new HomeScreen()));
        }

        private void OnReportButtonClick(object? sender, EventArgs e)
        {
            // Ask user where to save file
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Save Security Posture Report";
                sfd.Filter = "pdf (*.pdf)|*.pdf";
                sfd.ValidateNames = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // do a basic security check... make sure the path they chose is NOT on a network drive
                    var dirPath = Path.GetDirectoryName(sfd.FileName);
                    var isPathOnNetwork = IsFilePathOnNetwork(dirPath);

                    // first, check to make sure user is saving to local path
                    if (isPathOnNetwork)
                    {
                        MessageBox.Show("Unable to save to a network drive or removeable drive. Please save to a local drive");
                    }
                    // next, make sure the user entered a valid path
                    else if (!Directory.Exists(dirPath))
                    {
                        MessageBox.Show("Please enter a valid file path");
                    }
                    // save it
                    else
                    {
                        // generate a simple report by looping over scan results in the BL
                        StringBuilder sb = new StringBuilder();
                        foreach(var result in BL.Instance.ScanResults)
                        {
                            sb.AppendLine("=======================================");
                            sb.Append(result.ToString());
                            sb.AppendLine(string.Empty);
                        }

                        // generate random password.. 12 char for 54-bit equ
                        var password = GenerateRandomString(12);

                        // generate the report
                        BL.Instance.GenerateReport(sfd.FileName, password);

                        // inform user of password
                        MessageBox.Show($"Below is the password you will need to view your PDF report. Please memorize this password. Do not write down this password. If you forget this password, you can always rerun the scan.\r\n\r\n{password}", "Report Password");
                    }
                }
            }
        }

        private bool IsFilePathOnNetwork(string filePath)
        {
            bool isPathOnNetworkDrive = true; //default to safest answer
            try
            {
                FileInfo f = new FileInfo(filePath);
                string drive = Path.GetPathRoot(f.FullName);
                DriveInfo driveInfo = new DriveInfo(drive);
                isPathOnNetworkDrive = driveInfo.DriveType == DriveType.Network || driveInfo.DriveType == DriveType.Removable;
            }
            catch (Exception ex)
            {
                isPathOnNetworkDrive = true; // default to safest answer
            }
            return isPathOnNetworkDrive;
        }

        string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }

        public UserControl AsUserControl()
        {
            return this;
        }
    }
}
