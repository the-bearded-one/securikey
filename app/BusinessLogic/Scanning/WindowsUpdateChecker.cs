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

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRegularUpdates);

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
                            Console.WriteLine("Windows Auto Update setting could not be determined.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Windows Auto Update setting could not be determined.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingRegularUpdatesCompleted);

        }
    }

}
