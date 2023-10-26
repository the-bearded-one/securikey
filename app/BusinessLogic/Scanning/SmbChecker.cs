using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class SmbChecker : IChecker
    {
        public List<ScanResult> ScanResults => new List<ScanResult>();
        public bool IsServerEnabled { get; private set; } = false;
        public bool IsClientEnabled { get; private set; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingSmbEnabled);
            CheckSMBv1Client(@"SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters");
            CheckSMBv1Server(@"SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters");
            EventAggregator.Instance.FireEvent(BlEvents.CheckingSmbEnabledCompleted);
        }

        private void CheckSMBv1Client(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var dependOnService = key?.GetValue("DependOnService") as string[];
                    if (dependOnService != null && Array.Exists(dependOnService, element => element == "MRxSmb10"))
                    {
                        IsClientEnabled = true;
                    }
                    else
                    {
                        IsClientEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void CheckSMBv1Server(string subKey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    if (key != null)
                    {
                        IsServerEnabled = true;                        
                    }
                    else
                    {
                        IsServerEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
