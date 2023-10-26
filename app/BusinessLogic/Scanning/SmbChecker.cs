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
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool IsServerEnabled { get; private set; } = false;
        public bool IsClientEnabled { get; private set; } = false;

        public void Scan()
        {
            ScanResults.Clear();

            EventAggregator.Instance.FireEvent(BlEvents.CheckingSmbEnabled);
            CheckSMBv1Client(@"SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters");
            CheckSMBv1Server(@"SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters");

            ScanResult result = new ScanResult();
            result.ScanType = "Windows Services";
            result.DetailedDescription = $"Having the SMBv1 client enabled is risky because it lacks modern security features and is susceptible to a range of vulnerabilities, including man-in-the-middle attacks and ransomware exploits like WannaCry. Using it opens up your system to potential compromise.";
            if ( !IsClientEnabled )
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"SMBv1 client is not enabled.";
            }
            else 
            {
                result.Severity = Severity.High;
                result.ShortDescription = $"SMBv1 client is enabled!";
            }
            ScanResults.Add(result);

            ScanResult serverResult = new ScanResult();
            serverResult.ScanType = "Windows Services";
            serverResult.DetailedDescription = $"Having SMBv1 server enabled is a security risk because it's vulnerable to various types of attacks, most notably the EternalBlue exploit used by the WannaCry ransomware. This outdated protocol lacks modern security features, making your server a prime target for attackers.";
            if ( !IsServerEnabled )
            {
                serverResult.Severity = Severity.Ok;
                serverResult.ShortDescription = $"SMBv1 server is not enabled.";
            }
            else 
            {
                serverResult.Severity = Severity.High;
                serverResult.ShortDescription = $"SMBv1 server is enabled!";
            }
            ScanResults.Add(serverResult);


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
