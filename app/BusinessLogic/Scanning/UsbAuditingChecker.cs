using System;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using BusinessLogic.Scanning.Interfaces;
using Microsoft.Win32;

namespace BusinessLogic.Scanning
{
    public class UsbAuditingChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public List<SecurityCheck> SecurityResults { get; private set; } = new List<SecurityCheck>();

        public bool UsingAuditing { get; private set; } = false;

        public const String ID = "SK-38";
        public SecurityCheck SecurityCheck { get; private set; }

        public UsbAuditingChecker()
        {
            SecurityCheck = SecurityCheck.GetInstanceById(ID);
            SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.NotRun;
        }

        public void Scan()
        {
            ScanResults.Clear();
            SecurityResults.Clear();

            ProbeUsbAuditing();

            if ( SecurityCheck.Outcome != SecurityCheck.OutcomeTypes.Error)
            {
                if (UsingAuditing)
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Pass;
                }
                else
                {
                    SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.ActionRecommended;
                }
            }

            SecurityResults.Add(SecurityCheck);
            

        }

        private void ProbeUsbAuditing()
        {
            const string keyPath = @"SYSTEM\CurrentControlSet\Control\USBSTOR";
            const string valueName = "Start";

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
                        if (value != null && value is int)
                        {
                            int intValue = (int)value;
                            // Interpretation of the value depends on specific auditing configurations
                            Console.WriteLine($"USB Auditing setting: {intValue}");
                            UsingAuditing = true;
                        }
                        else
                        {
                            Console.WriteLine("USB Auditing setting not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("USB Auditing registry key not found. Disabled by default so it's not on");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                SecurityCheck.Outcome = SecurityCheck.OutcomeTypes.Error;
                SecurityCheck.ErrorMessage = ex.Message;
                // If the service is not installed, an exception will be thrown
                
            }

        }


    }
}
