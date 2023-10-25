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
                        IsAutoRunEnabled = true;                        
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
