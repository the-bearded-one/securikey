﻿using BusinessLogic;
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
        Control CurrentScreen = null;

        public SecuriKeyForm()
        {
            InitializeComponent();

            // Listen to BL events and display the right screen accordingly
            BL.Instance.EventAggregator.BlEvent += OnEventAggregatorBlEvent;

            // load initial screen
            LoadScreen(new HomeScreen());
        }

        private void LoadScreen(Control screen)
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
                    // fake smart navigation since this may be temporary
                    if (CurrentScreen.GetType() == typeof(ReportScreen))
                    {
                        // we want to know when new scan is requested
                        (CurrentScreen as ReportScreen).NewScanButtonClick -= OnSecuriKeyFormNewScanButtonClick;
                    }

                    Controls.Remove(CurrentScreen);
                    CurrentScreen.Dispose();
                }

                // load new screen
                CurrentScreen = screen;
                Controls.Add(CurrentScreen);
                CurrentScreen.Dock = DockStyle.Fill;

                // fake smart navigation since this may be temporary
                if (CurrentScreen.GetType() == typeof(ReportScreen))
                {
                    // we want to know when new scan is requested
                    (CurrentScreen as ReportScreen).NewScanButtonClick += OnSecuriKeyFormNewScanButtonClick;
                }
            }
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
                    if (!BL.Instance.FirewallChecker.IsFirewallEnabled)
                    {
                        status += $"\r\n    Firewall is not enabled!";
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
