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

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingNtlmV1Enabled);

            bool v1InUse = CheckNTLMv1Settings(@"SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0");

            if ( v1InUse )
            {
                ScanResult result = new ScanResult();
                result.ScanType = "NTLM Version";
                result.Severity = Severity.High;
                result.ShortDescription = "NTLMv1 in use";
                result.DetailedDescription = $"Disabling Remote Desktop Protocol (RDP) when not used can protect against security risks";
                ScanResults.Add(result);
            }
        
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
