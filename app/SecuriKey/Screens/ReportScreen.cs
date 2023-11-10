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
using static SkiaSharp.HarfBuzz.SKShaper;

namespace SecuriKey.Screens
{
    public partial class ReportScreen : UserControl, IScreen
    {
        string passwordPromptText = "Please enter the report password below. The password will be required to view the report in the future. The password must meet the following criteria:\r\n\r\n* 8 to 20 characters in length\r\n* at least 1 capital letter,\r\n* at least 1 lowercase letter\r\n* at least 1 number\r\n* at least 1 special character";

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
            string password = string.Empty;

            // ask user for password
            using (var passwordPrompt = new PopUpPrompt("Please Enter Password", passwordPromptText, InputValidation.Password))
            using (var sfd = new SaveFileDialog())
            {
                if (passwordPrompt.ShowDialog() == DialogResult.OK)
                {
                    password = passwordPrompt.Value;

                    // Ask user where to save file
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
                            // generate the report
                            BL.Instance.GenerateReport(sfd.FileName, password, openReportCheckBox.Checked);
                        }
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

        public UserControl AsUserControl()
        {
            return this;
        }
    }
}
