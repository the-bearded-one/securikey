using BusinessLogic;
using SecuriKey.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecuriKey
{
    public partial class SecuriKeyForm : Form
    {
        string status = string.Empty;
        IScreen CurrentScreen = null;

        public SecuriKeyForm()
        {
            InitializeComponent();

            // detect monitor size and resize to a reasonable size
            var displayScreen = Screen.FromControl(this);
            if (displayScreen != null)
            {
                if (displayScreen.Bounds.Width >= 1920)
                {
                    this.Width = 1920;
                    this.Height = 1080;
                }
                else if (displayScreen.Bounds.Width >= 1280)
                {
                    this.Width = 1280;
                    this.Height = 720;
                }
                else if (displayScreen.Bounds.Width >= 1024)
                {
                    this.Width = 1024;
                    this.Height = 768;
                }
                else
                {
                    this.Width = 800;
                    this.Height = 600;
                }
            }

            // listen to BL events and display the right screen accordingly
            BL.Instance.EventAggregator.BlEvent += OnEventAggregatorBlEvent;

            // load initial screen
            LoadScreen(new HomeScreen());
        }

        private void LoadScreen(IScreen screen)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { LoadScreen(screen); }));
            }
            else
            {
                // remove the old screen if any
                if (CurrentScreen != null)
                {
                    Controls.Remove(CurrentScreen.AsUserControl());
                    CurrentScreen.AsUserControl().Dispose();

                    // unsubscribe to old screen events
                    CurrentScreen.NavigationRequest -= OnNavigationRequest;
                }

                // load new screen
                CurrentScreen = screen;
                Controls.Add(CurrentScreen.AsUserControl());
                CurrentScreen.AsUserControl().Dock = DockStyle.Fill;

                // subscribe to new screen events
                CurrentScreen.NavigationRequest += OnNavigationRequest;
            }
        }

        private void OnNavigationRequest(object sender, NavigationEventArgs e)
        {
            LoadScreen(e.RequestedScreen);
        }

        private void OnSecuriKeyFormNewScanButtonClick(object? sender, EventArgs e)
        {
            LoadScreen(new HomeScreen());
        }

        private void OnEventAggregatorBlEvent(object? sender, BlEventArgs e)
        {
            status += $"\r\n{e.BlEvent.ToString()}";

            switch (e.BlEvent)
            {
                case BlEvents.SystemScanStarted:
                    // we starteed a system scan, make sure we are displaying the scanning screen
                    LoadScreen(new ScanningScreen());
                    break;
                case BlEvents.SystemScanCompleted:
                    // kludge: delay loading the report screen just a bit so user can see completed progress bar
                    System.Timers.Timer tm = new System.Timers.Timer();
                    tm.Interval = 4000;
                    tm.Elapsed += new System.Timers.ElapsedEventHandler((s, ev) =>
                    {
                        tm.Stop();
                        tm.Elapsed += null;
                        var reportScreen = new ReportScreen();
                        reportScreen.ReportText = status;
                        LoadScreen(reportScreen);
                    });
                    tm.Start();
                    break;
                case BlEvents.CveCheckCompleted:
                    status += $"\r\n    Found {BL.Instance.CveChecker.GetVulnerabilities().Count} possible CVE vulnerabilities";
                    break;
                case BlEvents.CheckingInternetStatusCompleted:
                    status += $"\r\n    System {(BL.Instance.InternetConnectionChecker.IsConnected ? "IS" : "is NOT")} connected to the internet";
                    break;
                case BlEvents.CheckingSecurityProductsCompleted:
                    status += $"\r\n    {BL.Instance.SecurityProductChecker.AntivirusProducts.Count} antivirus products found";
                    status += $"\r\n    {BL.Instance.SecurityProductChecker.AntispywareProducts.Count} antispyware products found";
                    status += $"\r\n    {BL.Instance.SecurityProductChecker.FirewallProducts.Count} firewall products found";
                    break;
                case BlEvents.CheckingApplicationVersionsCompleted:
                    foreach (var vulnerability in BL.Instance.AppScanner.VulnerabiltiesSeen)
                    {
                        status += $"\r\n    Vulnerable versions found! CVE ID: {vulnerability.CVE}";
                    }
                    break;
                case BlEvents.CheckingElevatedUserCompleted:
                    if (BL.Instance.UserType.IsElevatedUser)
                    {
                        status += $"\r\n    Running as an elevated user!";
                    }
                    break;
                case BlEvents.CheckingWindowsScriptingHostCompleted:
                    if (BL.Instance.WindowsScriptingHostChecker.IsWshEnabled)
                    {
                        status += $"\r\n    Windows Scripting Host (WSH) is enabled!";
                    }
                    break;
                case BlEvents.CheckingRdpEnabledCompleted:
                    if (BL.Instance.RdpChecker.IsRdpEnabled)
                    {
                        status += $"\r\n    Remote Desktop Protocol (RDP) is enabled!";
                    }
                    if (BL.Instance.RdpChecker.IsRdpWeak)
                    {
                        status += $"\r\n    Remote Desktop Protocol is using weak encryption!";
                    }
                    break;
                case BlEvents.CheckingSecureBootEnabledCompleted:
                    if (!BL.Instance.SecureBootChecker.IsSecureBootEnabled)
                    {
                        status += $"\r\n    SecureBoot is not enabled!";
                    }
                    break;
                case BlEvents.CheckingFirewallCompleted:
                    if (!BL.Instance.FirewallActiveChecker.IsFirewallEnabled)
                    {
                        status += $"\r\n    Firewall is not enabled!";
                    }
                    break;
                case BlEvents.CheckingNtlmV1EnabledComplete:                    
                    if (BL.Instance.NtlmChecker.IsNtmlV1InUse)
                    {
                        status += $"\r\n    NTLM v1 is enabled!";
                    }
                    break;
                case BlEvents.CheckingPageFileEncryptionCompleted:
                    if ( !BL.Instance.EncryptedPageFileChecker.IsPageFileEncrypted )
                    {
                        status += $"\r\n    PageFile is not encrypted!";
                    }
                    break;
                case BlEvents.CheckingSmbEnabledCompleted:
                    if (!BL.Instance.SmbChecker.IsServerEnabled)
                    {
                        status += $"\r\n    SMBv1 Server is enabled!";
                    }
                    if (!BL.Instance.SmbChecker.IsClientEnabled)
                    {
                        status += $"\r\n    SMBv1 Client is enabled!";
                    }
                    break;
                case BlEvents.CheckingWindowsVersionCompleted:
                    string windowsVersionInfoFormatted = string.Join("\r\n", BL.Instance.WindowsVersionChecker.VersionInfo.Select(kvp => $"    {kvp.Key}: {kvp.Value}"));
                    status += $"\r\n{windowsVersionInfoFormatted}";
                    if (BL.Instance.IsInternetConnectionAuthorized)
                    {
                        status += $"\r\n    Windows Update is Available";
                        foreach (string update in BL.Instance.WindowsVersionChecker.AvailableUpdates)
                        {
                            status += $"\r\n        {update}";
                        }
                    }
                    break;
            }
        }
    }
}
