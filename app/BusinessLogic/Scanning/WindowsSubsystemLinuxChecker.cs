using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;
using System;
using System.DirectoryServices;

namespace BusinessLogic.Scanning
{
    public class WindowsSubsystemLinuxChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool IsActive { get; private set; } = false;

        public SecurityCheck SecurityCheck { get; private set; }

        public const String ID = "SK-22";
        public WindowsSubsystemLinuxChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingWsl);

            IsActive = CheckWslActive();

            if (SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (IsActive)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
            }

            SecurityResults.Add(SecurityCheck);

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
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return true;
        }
        
    }
}
