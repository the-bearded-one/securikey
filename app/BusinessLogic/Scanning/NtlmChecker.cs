using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class NtlmChecker : IChecker
    {
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();

        public bool IsNtmlV1InUse { get; private set; } = false;
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingNtlmV1Enabled);

            IsNtmlV1InUse = CheckNTLMv1Settings(@"SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0");

            ScanResult result = new ScanResult();
            result.ScanType = "Windows Services";
            result.DetailedDescription = $"Having NTLMv1 enabled is a security risk because it uses weak cryptographic algorithms, making it susceptible to various attacks like relay and brute-force attacks. This could allow unauthorized access to network resources, compromising your system's security. Note that NTLMv1 isn't specifically tied to removable media; it's a network authentication protocol.";
            if (IsNtmlV1InUse)
            {
                result.Severity = Severity.Medium;
                result.ShortDescription = $"NTLMv1 is enabled!";
            }
            else
            {
                result.Severity = Severity.Ok;
                result.ShortDescription = $"NTLMv1 is not enabled";
            }
            ScanResults.Add(result);

            EventAggregator.Instance.FireEvent(BlEvents.CheckingNtlmV1EnabledComplete);

        }

        private bool CheckNTLMv1Settings(string subKey)
        {
            try
            {
                using (RegistryKey ntlmKey = Registry.LocalMachine.OpenSubKey(subKey, false))
                {
                    var minClientSec = ntlmKey?.GetValue("NtlmMinClientSec");
                    var minServerSec = ntlmKey?.GetValue("NtlmMinServerSec");

                    if (minClientSec != null && minServerSec != null)
                    {
                        uint clientSec = Convert.ToUInt32(minClientSec);
                        uint serverSec = Convert.ToUInt32(minServerSec);

                        if ((clientSec & 0x20000000) == 0 || (serverSec & 0x20000000) == 0)
                        {
                            return true;                            
                        }
                        else
                        {
                            return false;                            
                        }
                    }
                    else
                    {
                        // to do, reg key wasn't specifically defined so it is default setting for OS. need to check if v1 is default for win10 or 11
                        return false;                        
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
