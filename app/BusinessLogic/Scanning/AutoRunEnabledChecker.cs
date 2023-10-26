using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class AutoRunEnabledChecker : IChecker
    {

        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool IsAutoRunEnabled { get; private set; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingAutoRunEnabled);

            CheckAutoRun(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");

            ScanResult result = new ScanResult();
            result.ScanType = "Secure Defaults";
            result.DetailedDescription = $"Having AutoRun enabled for removable media is a security risk because it allows for the automatic execution of potentially malicious software the moment a removable device like a USB drive is plugged in. This makes it easier for malware to spread and infect your system.";
            if (IsAutoRunEnabled)
            {
                result.Severity = Severity.Critical;
                result.ShortDescription = $"Autorun is enabled for removable media!";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"Autorun is not enabled for removable media.";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingNtlmV1EnabledComplete);

        }

        private void CheckAutoRun(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var noDriveTypeAutoRun = key?.GetValue("NoDriveTypeAutoRun");

                    if (noDriveTypeAutoRun != null)
                    {
                        int value = Convert.ToInt32(noDriveTypeAutoRun);
                        // Checking if the 4th bit is 0 (which would mean AutoRun is enabled for removable drives)
                        if ((value & 0x08) == 0)
                        {
                            IsAutoRunEnabled = true;
                        }
                        else
                        {
                            IsAutoRunEnabled = false;
                        }
                    }
                    else
                    {
                        // no explicit registry key was defined which means it defaults to standard behavior.
                        // on windows 10/11, that means AutoRun is off
                        IsAutoRunEnabled = false;                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            };

        }


    }
}
