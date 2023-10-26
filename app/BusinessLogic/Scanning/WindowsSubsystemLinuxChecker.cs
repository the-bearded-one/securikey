using Microsoft.Win32;
using System;
using System.DirectoryServices;

namespace BusinessLogic.Scanning
{
    public class WindowsSubsystemLinuxChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool IsActive { get; private set; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingWsl);

            IsActive = CheckWslActive();

            ScanResult result = new ScanResult();
            result.ScanType = "Secure Defaults";
            result.DetailedDescription = $"Having WSL (Windows Subsystem for Linux) enabled on a system that doesn't require it can present an additional attack surface for potential intruders. WSL allows for the execution of Linux binaries, which could be exploited by an attacker to bypass Windows-specific security measures or to run malicious Linux software. By disabling unused features like WSL, you're effectively reducing the areas that need to be secured, thereby lowering the risk of a successful cyberattack.";
            if (IsActive)
            {
                result.Severity = Severity.Medium;
                result.ShortDescription = $"Windows Subsystem for Linux is enabled!";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Windows Subsystem for Linux is disabled";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingWslCompleted);

        }

        private bool CheckWslActive()
        {
            try
            {
                // Path to the registry location where Windows features are listed
                string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Packages";

                // Open the registry key (hive)
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    // Open the subkey containing Windows features
                    using (RegistryKey subKey = baseKey.OpenSubKey(registryPath))
                    {
                        // Check if subKey exists
                        if (subKey != null)
                        {
                            // Loop through each subkey to find WSL
                            foreach (string keyName in subKey.GetSubKeyNames())
                            {
                                if (keyName.Contains("Microsoft-Windows-Subsystem-Linux"))
                                {
                                    using (RegistryKey wslKey = subKey.OpenSubKey(keyName))
                                    {
                                        // Check if WSL is enabled
                                        if (wslKey != null && wslKey.GetValue("CurrentState") != null)
                                        {
                                            int currentState = Convert.ToInt32(wslKey.GetValue("CurrentState"));
                                            if (currentState == 7) // Generally, a value of 7 means enabled
                                            {
                                                return false; // Exit the program
                                            }
                                        }
                                    }
                                }
                            }
                            // If the loop completes and we haven't returned, WSL is either not installed or not enabled
                            Console.WriteLine("Windows Subsystem for Linux is not enabled.");
                        }
                        else
                        {
                            Console.WriteLine("Could not access registry to check WSL status.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return true;
        }
        
    }
}
