using BusinessLogic.Scanning.Interfaces;
using BusinessLogic.Scanning.POCOs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class WindowsUpdateChecker : IChecker
    {
        public bool AreRegularUpdatesEnabled { get; private set; } = false;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();
        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-23";
        public WindowsUpdateChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRegularUpdates);

            ProbeWindowsUpdate();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (AreRegularUpdatesEnabled)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
            }
            SecurityResults.Add(SecurityCheck);
            EventAggregator.Instance.FireEvent(BlEvents.CheckingRegularUpdatesCompleted);

        }

        private void ProbeWindowsUpdate()
        {
            try
            {
                // Open the registry key for reading
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update", false))
                {
                    if (key != null)
                    {
                        // Read the AUOptions value from the key
                        Object val = key.GetValue("AUOptions");
                        if (val != null)
                        {
                            int setting = Convert.ToInt32(val);
                            switch (setting)
                            {
                                case 2:
                                case 3:
                                case 4:
                                    AreRegularUpdatesEnabled = true;
                                    {
                                        ScanResult result = new ScanResult();
                                        result.ScanType = "Windows Update";
                                        result.Severity = Severity.Ok;
                                        result.ShortDescription = "Windows Auto Update is enabled";
                                        result.DetailedDescription = $"Windows auto-update is important because it automatically delivers critical security patches and system updates, reducing the risk of malware infections, cyberattacks, and data breaches. It ensures timely fixes for vulnerabilities.";
                                        ScanResults.Add(result);
                                    }
                                    Console.WriteLine("Windows Auto Update is enabled.");
                                    break;
                                default:
                                    AreRegularUpdatesEnabled = false;
                                    {
                                        ScanResult result = new ScanResult();
                                        result.ScanType = "Windows Update";
                                        result.Severity = Severity.Medium;
                                        result.ShortDescription = "Windows Auto Update is disabled";
                                        result.DetailedDescription = $"Windows auto-update is important because it automatically delivers critical security patches and system updates, reducing the risk of malware infections, cyberattacks, and data breaches. It ensures timely fixes for vulnerabilities.";
                                        ScanResults.Add(result);
                                    }
                                    Console.WriteLine("Windows Auto Update is disabled.");
                                    break;
                            }
                        }
                        else
                        {
                            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                            Console.WriteLine("Windows Auto Update setting could not be determined.");
                        }
                    }
                    else
                    {
                        SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                        Console.WriteLine("Windows Auto Update setting could not be determined.");
                    }
                }
            }
            catch (Exception ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
    }

}
