﻿using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void OnAuthorizeInternetConnectionCheckboxCheckedChanged(object sender, EventArgs e)
        {
            BL.Instance.IsInternetConnectionAuthorized = authorizeInternetConnectionCheckbox.Checked;
        }

        private void OnBlEvent(object? sender, BlEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { OnBlEvent(sender, e); }));
                return;
            }

            _statusTextbox.Text += $"\r\n{e.BlEvent.ToString()}";

            switch (e.BlEvent)
            {
                case BlEvents.SystemScanStarted:
                    _startButton.Enabled = false;
                    break;
                case BlEvents.SystemScanStopped:
                    _startButton.Enabled = true;
                    break;
                case BlEvents.SystemScanAbortedError:
                    _startButton.Enabled = true;
                    break;
                case BlEvents.SystemScanCompleted:
                    _startButton.Enabled = true;
                    break;
                case BlEvents.CveCheckCompleted:
                    _statusTextbox.Text += $"\r\n    Found {BL.Instance.CveChecker.GetVulnerabilities().Count} possible CVE vulnerabilities";
                    break;
                case BlEvents.CheckingInternetStatusCompleted:
                    _statusTextbox.Text += $"\r\n    System {(BL.Instance.InternetConnectionChecker.IsConnected ? "IS" : "is NOT")} connected to the internet";
                    break;
                case BlEvents.CheckingSecurityProductsCompleted:
                    _statusTextbox.Text += $"\r\n    {BL.Instance.SecurityProductChecker.AntivirusProducts.Count} antivirus products found";
                    _statusTextbox.Text += $"\r\n    {BL.Instance.SecurityProductChecker.AntispywareProducts.Count} antispyware products found";
                    _statusTextbox.Text += $"\r\n    {BL.Instance.SecurityProductChecker.FirewallProducts.Count} firewall products found";
                    break;
                case BlEvents.CheckingWindowsVersionCompleted:
                    string windowsVersionInfoFormatted = string.Join("\r\n", BL.Instance.WindowsVersionChecker.VersionInfo.Select(kvp => $"    {kvp.Key}: {kvp.Value}"));
                    _statusTextbox.Text += $"\r\n{windowsVersionInfoFormatted}";
                    if (BL.Instance.IsInternetConnectionAuthorized)
                    {
                        _statusTextbox.Text += $"\r\n    Windows Update is Available";
                        foreach (string update in BL.Instance.WindowsVersionChecker.AvailableUpdates)
                        {
                            _statusTextbox.Text += $"\r\n        {update}";
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
