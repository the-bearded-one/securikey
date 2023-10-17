using BusinessLogic;
using System.Data;

namespace SecuriKey
{
    public partial class mainForm : Form
    {
        #region Constructor

        public mainForm()
        {
            InitializeComponent();

            // Listen for BL events=
            BL.Instance.EventAggregator.BlEvent += OnBlEvent;

            // Init states
            BL.Instance.IsInternetConnectionAuthorized = authorizeInternetConnectionCheckbox.Checked;
        }

        #endregion

        # region Event Handlers

        private void OnStartButtonClick(object sender, EventArgs e)
        {
            BL.Instance.StartSystemScan();
        }

        private void OnStopButtonClick(object sender, EventArgs e)
        {
            BL.Instance.StopSystemScan();
        }

        private void OnGenerateReportButtonClick(object sender, EventArgs e)
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
                        BL.Instance.GenerateReport(sfd.FileName, statusTextbox.Text);
                    }
                }
            }
        }

        private void OnAuthorizeInternetConnectionCheckboxCheckedChanged(object sender, EventArgs e)
        {
            BL.Instance.IsInternetConnectionAuthorized = authorizeInternetConnectionCheckbox.Checked;
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

        private void OnBlEvent(object? sender, BlEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { OnBlEvent(sender, e); }));
                return;
            }

            statusTextbox.Text += $"\r\n{e.BlEvent.ToString()}";

            switch (e.BlEvent)
            {
                case BlEvents.SystemScanStarted:
                    startButton.Enabled = false;
                    break;
                case BlEvents.SystemScanStopped:
                    startButton.Enabled = true;
                    break;
                case BlEvents.SystemScanAbortedError:
                    startButton.Enabled = true;
                    break;
                case BlEvents.SystemScanCompleted:
                    startButton.Enabled = true;
                    break;
                case BlEvents.CveCheckCompleted:
                    statusTextbox.Text += $"\r\n    Found {BL.Instance.CveChecker.GetVulnerabilities().Count} possible CVE vulnerabilities";
                    break;
                case BlEvents.CheckingInternetStatusCompleted:
                    statusTextbox.Text += $"\r\n    System {(BL.Instance.InternetConnectionChecker.IsConnected ? "IS" : "is NOT")} connected to the internet";
                    break;
                case BlEvents.CheckingSecurityProductsCompleted:
                    statusTextbox.Text += $"\r\n    {BL.Instance.SecurityProductChecker.AntivirusProducts.Count} antivirus products found";
                    statusTextbox.Text += $"\r\n    {BL.Instance.SecurityProductChecker.AntispywareProducts.Count} antispyware products found";
                    statusTextbox.Text += $"\r\n    {BL.Instance.SecurityProductChecker.FirewallProducts.Count} firewall products found";
                    break;
                case BlEvents.CheckingApplicationVersionsCompleted:
                    break;
                case BlEvents.CheckingElevatedUserCompleted:
                    if (BL.Instance.UserType.IsElevatedUser)
                    {
                        statusTextbox.Text += $"\r\n    Running as an elevated user!";
                    }
                    break;
                case BlEvents.CheckingWindowsScriptingHostCompleted:
                    if (BL.Instance.WindowsScriptingHostChecker.IsWshEnabled)
                    {
                        statusTextbox.Text += $"\r\n    Windows Scripting Host (WSH) is enabled!";
                    }
                    break;
                case BlEvents.CheckingRdpEnabledCompleted:
                    if (BL.Instance.RdpChecker.IsRdpEnabled)
                    {
                        statusTextbox.Text += $"\r\n    Remote Desktop Protocol (RDP) is enabled!";
                    }
                    if (BL.Instance.RdpChecker.IsRdpWeak)
                    {
                        statusTextbox.Text += $"\r\n    Remote Desktop Protocol is using weak encryption!";
                    }
                    break;
                case BlEvents.CheckingRegularUpdatesCompleted:
                    if (!BL.Instance.WindowsUpdateChecker.AreRegularUpdatesEnabled)
                    {
                        statusTextbox.Text += $"\r\n    Automatic Windows Updates are not enabled!";
                    }
                    break;

                case BlEvents.CheckingSecureBootEnabledCompleted:
                    if (!BL.Instance.SecureBootChecker.IsSecureBootEnabled)
                    {
                        statusTextbox.Text += $"\r\n    SecureBoot is not enabled!";
                    }
                    break;
                case BlEvents.CheckingFirewallCompleted:
                    if (!BL.Instance.FirewallChecker.IsFirewallEnabled)
                    {
                        statusTextbox.Text += $"\r\n    Firewall is not enabled!";
                    }
                    break;
                case BlEvents.CheckingBitLockerCompleted: // requires admin to test
                    if (BL.Instance.BitLockerChecker.IsStatusProtected == false)
                    {
                        if (BL.Instance.BitLockerChecker.IsBitLockerSupported && !BL.Instance.BitLockerChecker.IsBitLockerEnabled)
                        {
                            statusTextbox.Text += $"\r\n    BitLocker is supported but not enabled!";
                        }
                    }
                    break;
                case BlEvents.CheckingUacCompleted: // requires admin to test
                    if (BL.Instance.UacChecker.UnableToQuery == false)
                    {
                        if (BL.Instance.UacChecker.IsUacDisabled)
                        {
                            statusTextbox.Text += $"\r\n    User Access Control (UAC) is disabled!";
                        } else if (!BL.Instance.UacChecker.IsUacAtRecommendedLevel)
                        {
                            statusTextbox.Text += $"\r\n    User Access Control (UAC) is enabled but not at recommended level!";
                        }
                    }
                    break;
                case BlEvents.CheckingPowerShellExecutionPolicyCompleted:
                    if (BL.Instance.PowerShellChecker.UnableToQuery == false)
                    {
                        if (BL.Instance.PowerShellChecker.HasWeakExecutionPolicy)
                        {
                            statusTextbox.Text += $"\r\n    PowerShell Execution Policy is Weak!";
                        }
                    }
                    break;
                case BlEvents.CheckingWindowsVersionCompleted:
                    string windowsVersionInfoFormatted = string.Join("\r\n", BL.Instance.WindowsVersionChecker.VersionInfo.Select(kvp => $"    {kvp.Key}: {kvp.Value}"));
                    statusTextbox.Text += $"\r\n{windowsVersionInfoFormatted}";
                    if (BL.Instance.IsInternetConnectionAuthorized)
                    {
                        statusTextbox.Text += $"\r\n    Windows Update is Available";
                        foreach (string update in BL.Instance.WindowsVersionChecker.AvailableUpdates)
                        {
                            statusTextbox.Text += $"\r\n        {update}";
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
